using FarPoint.Win.Spread;
using Metroit.Collections.Generic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Metroit.Win.GcSpread.MultiRow.Collections.Generic
{
    /// <summary>
    /// 1レコードを複数行として扱う機能を提供します。
    /// null が許容されるセルのときに、アイテムが null 許容型でない場合、セルの見た目とアイテムの値が一致しない可能性があります。
    /// </summary>
    public class MultiRowSheet<T> : IMultiRowSheet<T>, ISheetState, IDisposable where T : StateKnownMultiRowItemBase, new()
    {
        /// <summary>
        /// Cell の値が変更されたかどうかを取得または設定します。
        /// </summary>
        protected internal bool IsCellValueChanged { get; protected set; } = false;

        bool ISheetState.IsCellValueChanged => IsCellValueChanged;

        /// <summary>
        /// 扱っているシートを取得します。
        /// </summary>
        public SheetView Sheet { get; protected set; }

        /// <summary>
        /// 1レコードに対する行数を取得します。
        /// </summary>
        public int RowNumber { get; protected set; }

        /// <summary>
        /// アイテムデータを取得します。
        /// 外部からの利用は不要です。
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        IEnumerable<object> Collections.IMultiRowSheet.Rows => Rows;

        /// <summary>
        /// アイテムデータを取得します。
        /// </summary>
        public IReadOnlyList<T> Rows => _rows;

        /// <summary>
        /// アイテムデータを取得する。
        /// </summary>
        private ItemRemovedKnownList<T> _rows = new ItemRemovedKnownList<T>();

        private Action<int, Cell> CellSetup { get; }

        /// <summary>
        /// 新しいインスタンスを生成します。
        /// </summary>
        /// <param name="sheet">シートオブジェクト。</param>
        /// <param name="rowNumber">1レコードの行数。</param>
        /// <param name="cellSetup">行の追加が行われた時、セルの CellType や Tag の設定, セル結合などを行い、行のセル情報設定します。</param>
        public MultiRowSheet(SheetView sheet, int rowNumber, Action<int, Cell> cellSetup = null)
        {
            Sheet = sheet;
            Sheet.CellChanged += Sheet_CellChanged;

            RowNumber = rowNumber;
            CellSetup = cellSetup;
        }

        /// <summary>
        /// 行ヘッダーと背景色を描画する。
        /// </summary>
        /// <param name="rowIndex">データのインデックス。</param>
        private void DrawRowStyle(int rowIndex)
        {
            var startRow = rowIndex * RowNumber;
            var endRow = startRow + RowNumber - 1;

            // 行ヘッダーを結合し、カウントを変更
            Sheet.AddRowHeaderSpanCell(startRow, 0, RowNumber, 1);
            Sheet.RowHeader.Cells[startRow, 0].Value = rowIndex + 1;

            // 奇数行/偶数行で背景色の変更
            if ((rowIndex + 1) % 2 == 0)
            {
                Sheet.Rows[startRow, endRow].BackColor = Sheet.AlternatingRows.Cast<AlternatingRow>().Last().BackColor;
            }
            else
            {
                Sheet.Rows[startRow, endRow].BackColor = Sheet.AlternatingRows.Cast<AlternatingRow>().First().BackColor;
            }
        }





        /// <summary>
        /// 新しいアイテムを生成します。
        /// </summary>
        /// <returns>新しいアイテム。</returns>
        object Collections.IMultiRowSheet.NewItem()
        {
            return NewItem();
        }

        /// <summary>
        /// 新しいアイテムを生成します。
        /// </summary>
        /// <returns>新しいアイテム。</returns>
        public T NewItem()
        {
            return new T();
        }

        private bool rowAdding = false;

        /// <summary>
        /// 行を追加します。
        /// </summary>
        /// <returns>追加された行のアイテム。</returns>
        object Collections.IMultiRowSheet.AddRow() => AddRow();

        /// <summary>
        /// 行を追加します。
        /// </summary>
        /// <returns>追加された行のアイテム。</returns>
        public T AddRow()
        {
            return AddRow(NewItem());
        }

        /// <summary>
        /// 行を追加します。
        /// 外部からの利用は不要です。
        /// </summary>
        /// <param name="row">追加するアイテム。</param>
        /// <returns>追加されたアイテム</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public object AddRow(object row) => AddRow((T)row);

        /// <summary>
        /// 行を追加します。
        /// </summary>
        /// <param name="row">追加するアイテム。</param>
        /// <returns>追加されたアイテム</returns>
        public T AddRow(T row)
        {
            rowAdding = true;

            InitializeItem(row);
            AddActualRow(row);

            // アイテムの値を実際のセルへ反映する
            var pis = row.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty);
            foreach (var pi in pis)
            {
                row.ReactiveCellValue(pi.Name, pi.GetValue(row));
            }

            _rows.Add(row);
            rowAdding = false;

            return row;
        }

        /// <summary>
        /// 行を追加します。
        /// 外部からの利用は不要です。
        /// </summary>
        /// <param name="rows">追加するアイテム。</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void AddRowRange(IEnumerable<object> rows) => AddRowRange((IEnumerable<T>)rows);

        /// <summary>
        /// 行を追加します。
        /// </summary>
        /// <param name="rows">追加するアイテム。</param>
        public void AddRowRange(IEnumerable<T> rows)
        {
            foreach (var row in rows)
            {
                AddRow(row);
            }
        }

        /// <summary>
        /// アイテムの初期化を行う。
        /// </summary>
        /// <param name="item">アイテム。</param>
        private void InitializeItem(T item)
        {
            item.Initialize(Sheet, this);
        }

        /// <summary>
        /// 実際のシート行を追加する。
        /// </summary>
        /// <param name="item">行のアイテム。</param>
        private void AddActualRow(T item)
        {
            Sheet.Rows.Add(Sheet.Rows.Count, RowNumber);

            // 行番号の割当と行の背景色を変更する
            DrawRowStyle(_rows.Count);

            // 追加されたすべての行の Tag に、行オブジェクトを設定し、セルセットアップを実施する
            for (var actualRowIndex = Sheet.Rows.Count - RowNumber; actualRowIndex <= Sheet.Rows.Count - 1; actualRowIndex++)
            {
                Sheet.RowHeader.Rows[actualRowIndex].Tag = item;
                if (CellSetup == null)
                {
                    continue;
                }

                for (var columnIndex = 0; columnIndex < Sheet.Columns.Count; columnIndex++)
                {
                    CellSetup.Invoke(GetItemRowIndex(actualRowIndex), Sheet.Cells[actualRowIndex, columnIndex]);
                }
            }
        }

        /// <summary>
        /// 画面の行インデックスから、行を削除する。
        /// </summary>
        /// <param name="index"></param>
        public void RemoveRow(int index)
        {
            var item = (T)Sheet.RowHeader.Rows[index].Tag;
            RemoveRow(item);
        }

        /// <summary>
        /// 行を削除します。
        /// 外部からの利用は不要です。
        /// </summary>
        /// <param name="row">削除するアイテム。</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        void Collections.IMultiRowSheet.RemoveRow(object row) => RemoveRow((T)row);

        /// <summary>
        /// 行を削除します。
        /// </summary>
        /// <param name="row">削除するオブジェクト。</param>
        public void RemoveRow(T row)
        {
            var actualRowIndex = _rows.IndexOf(row) * RowNumber;
            Sheet.Rows.Remove(actualRowIndex, RowNumber);
            _rows.Remove(row);

            foreach (var redrawRow in Rows.Select((Item, Index) => new { Item, Index }))
            {
                DrawRowStyle(redrawRow.Index);
            }
        }

        /// <summary>
        /// アイテムのインデックスから、行を削除します。
        /// </summary>
        /// <param name="index">アイテムのインデックス。</param>
        public void RemoveRowItemIndex(int index)
        {
            RemoveRow(_rows[index]);
        }

        /// <summary>
        /// すべての要素をクリアします。
        /// 削除されたことは通知されません。
        /// </summary>
        public void Clear()
        {
            if (_rows.Count > 0)
            {
                var rowCount = ((_rows.Count - 1) * RowNumber) + RowNumber;
                Sheet.Rows.Remove(0, rowCount);
            }

            _rows.Clear();
        }

        /// <summary>
        /// 画面の行インデックスから、アイテムのインデックスを取得します。
        /// </summary>
        /// <param name="index">画面の行インデックス。</param>
        /// <returns>アイテムのインデックス。</returns>
        public int GetItemIndex(int index)
        {
            return (int)Math.Truncate(index / Convert.ToDecimal(RowNumber));
        }

        /// <summary>
        /// 画面の行インデックスから、アイテムの MultiRowAttribute で指定された行インデックスを取得します。
        /// </summary>
        /// <param name="index">画面の行インデックス。</param>
        /// <returns>アイテムの MultiRowAttribute で指定された行インデックス。</returns>
        public int GetItemRowIndex(int index)
        {
            return index % RowNumber;
        }

        /// <summary>
        /// 画面の行インデックスから、アイテムを取得します。
        /// 外部からの利用は不要です。
        /// </summary>
        /// <param name="index">画面の行インデックス。</param>
        /// <returns>アイテム。</returns>
        object Collections.IMultiRowSheet.GetItem(int index) => GetItem(index);

        /// <summary>
        /// 画面の行インデックスから、アイテムを取得します。
        /// </summary>
        /// <param name="index">画面の行インデックス。</param>
        /// <returns>アイテム。</returns>
        public T GetItem(int index)
        {
            return (T)Sheet.RowHeader.Rows[index].Tag;
        }

        private bool disposed = false;

        /// <summary>
        /// オブジェクトを破棄する時、イベントを除去する。
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// オブジェクトの破棄を行う。
        /// </summary>
        /// <param name="disposing">マネージコードを破棄するかどうか。</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }

            if (disposing)
            {
                Sheet.CellChanged -= Sheet_CellChanged;
            }

            disposed = true;
        }

        /// <summary>
        /// セルの値が変更された時、値を MultiRowIteBase へ反映する。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Sheet_CellChanged(object sender, SheetViewEventArgs e)
        {
            // AddRowによる走行時は処理しない
            if (rowAdding)
            {
                return;
            }

            // 目的のセルがない場合は処理しない
            if (e.Row < 0 || e.Column < 0)
            {
                return;
            }

            var item = GetItem(e.Row);

            // アイテムの値が変更された時の走行時は処理しない
            if (item.IsItemValueChanged)
            {
                return;
            }

            var rowType = item.GetType();

            // 対象列インデックスのプロパティに値を反映する
            var pi = rowType
                .GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.SetProperty)
                .Where(x =>
                {
                    var attr = x.GetCustomAttribute(typeof(MultiRowAttribute)) as MultiRowAttribute;
                    if (attr == null)
                    {
                        return false;
                    }
                    if (attr.Row != GetItemRowIndex(e.Row) || attr.Column != e.Column)
                    {
                        return false;
                    }
                    return true;
                })
                .FirstOrDefault();
            if (pi == null)
            {
                return;
            }

            IsCellValueChanged = true;

            var targetType = pi.PropertyType;
            var safeType = Nullable.GetUnderlyingType(pi.PropertyType);
            if (safeType != null)
            {
                targetType = safeType;
            }
            var value = Convert.ChangeType(Sheet.Cells[e.Row, e.Column].Value, targetType);

            pi.SetValue(item, value);

            IsCellValueChanged = false;
        }
    }
}
