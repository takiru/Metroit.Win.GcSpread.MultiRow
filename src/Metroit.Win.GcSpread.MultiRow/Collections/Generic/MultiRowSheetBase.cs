using FarPoint.Win.Spread;
using Metroit.Collections.Generic;
using Metroit.CommunityToolkit.Mvvm;
using Metroit.Win.GcSpread.MultiRow.Metroit.ChangeTracking;
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
    /// <typeparam name="T">状態を持つ変更追跡が可能なクラス。</typeparam>
    //public class MultiRowSheet<T> : IMultiRowSheet<T>, IDisposable where T : TrackingObservableObjectEx, new()
    //public class MultiRowSheet<T> : IDisposable where T : TrackingObservableObjectEx<T>, new()
    //public class MultiRowSheet<T> : IDisposable where T : TrackingObservableObject<T>, IStateObject, new()
    //public class MultiRowSheet<T> : IDisposable where T : class, IPropertyChangeTracker<T>, IStateObject, new()
    public abstract class MultiRowSheetBase<T> : IDisposable where T : TrackingObjectWithState<T>, new()
    {
        /// <summary>
        /// 扱っているシートを取得します。
        /// </summary>
        public SheetView Sheet { get; protected set; }

        /// <summary>
        /// 1レコードに対する行数を取得します。
        /// </summary>
        public int RowsPerRecord { get; protected set; }

        private TrackingList<T> _list;

        public IReadOnlyList<T> Rows => _list;

        /// <summary>
        /// 新しいインスタンスを生成します。
        /// </summary>
        /// <param name="sheet">シートオブジェクト。</param>
        /// <param name="list">取り扱うリスト。</param>
        /// <param name="rowsPerRecord">1レコードの行数。</param>
        /// <param name="cellSetup">行の追加が行われた時、セルの CellType や Tag の設定, セル結合などを行い、行のセル情報設定します。</param>
        public MultiRowSheetBase(SheetView sheet, int rowsPerRecord, TrackingList<T> list, Action<int, Cell> cellSetup = null)
        {
            Sheet = sheet;
            Sheet.CellChanged += Sheet_CellChanged;

            RowsPerRecord = rowsPerRecord;
            _list = list;
            _list.ListChanged += _list_ListChanged;

            CellSetup = cellSetup;
        }

        /// <summary>
        /// どのような操作によってアクションが行われたか。
        /// </summary>
        private ActionBeginOperation _actionBeginOperation = ActionBeginOperation.None;


        private void _list_ListChanged(object sender, ListChangedEventArgs e)
        {
            switch (e.ListChangedType)
            {
                case ListChangedType.ItemAdded:
                    // Add(), AddNew(), Insert() で走行する
                    AddRow(e.NewIndex);
                    break;

                case ListChangedType.ItemChanged:
                    // ResetItem(), INotifyPropertyChangedによって値変更が通知されたときに走行する
                    // ResetItem() による制御は行わない。
                    // NOTE: ResetItem() で走行する場合、OldIndexは -1 になる。
                    ChangeRowItem(e.NewIndex);
                    break;

                case ListChangedType.Reset:
                    // Clear() で走行する
                    ClearActualRows();
                    break;

                case ListChangedType.ItemDeleted:
                    // Remove(), RemoveAt(), CancelNew() で走行する
                    RemoveActualRow();
                    break;

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rowIndex"></param>
        private void AddRow(int rowIndex)
        {
            _actionBeginOperation = ActionBeginOperation.Item;

            if (!_list.Last().ChangeTracker.IsTracking)
            {
                _list.Last().ChangeTracker.Reset();
            }

            AddActualRow((T)_list[rowIndex]);

            // アイテムの値を実際のセルへ反映する
            var pis = ((T)_list[rowIndex]).GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty);
            foreach (var pi in pis)
            {
                ReactiveCellValue((T)_list[rowIndex], pi);
            }

            _actionBeginOperation = ActionBeginOperation.None;
        }

        /// <summary>
        /// リストのアイテムに
        /// </summary>
        /// <param name="rowIndex"></param>
        private void ChangeRowItem(int rowIndex)
        {
            // NOTE: Sheet_CellChanged から制御が移ったときは行わない
            if (_actionBeginOperation == ActionBeginOperation.None)
            {
                _actionBeginOperation = ActionBeginOperation.Item;
                // 直前で変更されたプロパティの値を実際のセルへ反映する
                var pi = ((T)_list[rowIndex]).GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty)
                    .Where(x => x.Name == ((T)_list[rowIndex]).ChangeTracker.LastTrackingProperty)
                    .Single();
                ReactiveCellValue((T)_list[rowIndex], pi);
            }

            _actionBeginOperation = ActionBeginOperation.None;
        }

        /// <summary>
        /// セルの値が変更された時、値を MultiRowIteBase へ反映する。
        /// 値の変更後の制御は ChangeRowItem にて実施する。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Sheet_CellChanged(object sender, SheetViewEventArgs e)
        {
            // アイテムからの操作のときは何もしない
            if (_actionBeginOperation == ActionBeginOperation.Item)
            {
                return;
            }

            // 目的のセルがない場合は処理しない
            if (e.Row < 0 || e.Column < 0)
            {
                return;
            }

            _actionBeginOperation = ActionBeginOperation.ActualCell;
            var item = GetItem(e.Row);

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

            var targetType = Nullable.GetUnderlyingType(pi.PropertyType) ?? pi.PropertyType;
            var value = Convert.ChangeType(Sheet.Cells[e.Row, e.Column].Value, targetType);

            pi.SetValue(item, value);
        }

        /// <summary>
        /// 実際の行を削除する。
        /// </summary>
        private void RemoveActualRow()
        {
            var actualStartRowIndex = GetRemoveActualStartRowIndex();
            Sheet.Rows.Remove(actualStartRowIndex, RowsPerRecord);
            ReDrawBelowRows(actualStartRowIndex);
        }

        /// <summary>
        /// 実際の行をすべて削除する。
        /// </summary>
        private void ClearActualRows()
        {
            Sheet.Rows.Clear();
        }

        /// <summary>
        /// 削除指示が行われた1レコードの実際の開始行インデックスを取得する。
        /// </summary>
        /// <returns>削除指示が行われた1レコードの実際の開始行インデックス。</returns>
        private int GetRemoveActualStartRowIndex()
        {
            var actualRows = Sheet.RowHeader.Rows.Cast<Row>().Select((Value, Index) => new { Index, Value });

            Func<T, int> getActualStartRowIndex = (T item) =>
            {
                return actualRows.Where(x => x.Value.Tag == _list.LastAccessItem).First().Index;
            };

            if (_list.LastAccessItem.State == ItemState.New)
            {
                return getActualStartRowIndex(_list.LastAccessItem);
            }

            if (_list.LastAccessItem.State == ItemState.NewModified)
            {
                return getActualStartRowIndex(_list.LastAccessItem);
            }

            return getActualStartRowIndex(_list.Removed.Last());
        }

        /// <summary>
        /// <paramref name="actualStartRowIndex"/> を含む下の行を描画し直す。
        /// </summary>
        /// <param name="actualStartRowIndex">1レコードの実際の開始行インデックス。</param>
        private void ReDrawBelowRows(int actualStartRowIndex)
        {
            foreach (var row in Sheet.Rows.Cast<Row>()
                .Select((Row, Index) => new { Row, Index })
                .Where(x => x.Index >= actualStartRowIndex && x.Index % RowsPerRecord == 0))
            {
                SetRowNumber(row.Index);
                ChangeBackgroundColor(row.Index);
            }
        }

        /// <summary>
        /// 実際のシート行を追加する。
        /// </summary>
        /// <param name="item">行のアイテム。</param>
        private void AddActualRow(T item)
        {
            Sheet.Rows.Add(Sheet.Rows.Count, RowsPerRecord);

            // 行番号の割当と行の背景色を変更する
            var actualRowIndex2 = Sheet.Rows.Count - RowsPerRecord;
            DrawRowStyle(actualRowIndex2);

            // 追加されたすべての行の Tag に、行オブジェクトを設定し、セルセットアップを実施する
            for (var actualRowIndex = Sheet.Rows.Count - RowsPerRecord; actualRowIndex <= Sheet.Rows.Count - 1; actualRowIndex++)
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
        /// 行ヘッダーと背景色を描画する。
        /// </summary>
        /// <param name="actualStartRowIndex">1レコードの実際の開始行インデックス。</param>
        private void DrawRowStyle(int actualStartRowIndex)
        {
            var rowNumber = CreateActualRowNumber(actualStartRowIndex);
            var actualEndRowIndex = actualStartRowIndex + RowsPerRecord - 1;

            // 行ヘッダーを結合
            Sheet.AddRowHeaderSpanCell(actualStartRowIndex, 0, RowsPerRecord, 1);
            SetRowNumber(actualStartRowIndex);
            ChangeBackgroundColor(actualStartRowIndex);
        }

        /// <summary>
        /// 1レコードの実際の行番号を取得する。
        /// </summary>
        /// <param name="actualStartRowIndex">1レコードの実際の開始行インデックス。</param>
        /// <returns>1レコードの実際の行番号。</returns>
        private int CreateActualRowNumber(int actualStartRowIndex)
        {
            return actualStartRowIndex / RowsPerRecord;
        }

        /// <summary>
        /// 1レコードの行ヘッダーに表示する行番号を設定する。
        /// </summary>
        /// <param name="actualStartRowIndex">1レコードの実際の開始行インデックス。</param>
        private void SetRowNumber(int actualStartRowIndex)
        {
            var rowNumber = CreateActualRowNumber(actualStartRowIndex);
            var actualEndRowIndex = actualStartRowIndex + RowsPerRecord - 1;

            for (var i = actualStartRowIndex; i <= actualEndRowIndex; i++)
            {
                Sheet.RowHeader.Cells[i, 0].Value = rowNumber + 1;
            }
        }

        /// <summary>
        /// 1レコードの行の背景色を変更する。
        /// </summary>
        /// <param name="actualStartRowIndex">1レコードの実際の開始行インデックス。</param>
        private void ChangeBackgroundColor(int actualStartRowIndex)
        {
            var actualEndRowIndex = actualStartRowIndex + RowsPerRecord - 1;

            // 奇数行/偶数行で背景色の変更
            var rowNumber = CreateActualRowNumber(actualStartRowIndex); ;
            if (rowNumber % RowsPerRecord == 0)
            {
                Sheet.Rows[actualStartRowIndex, actualEndRowIndex].BackColor = Sheet.AlternatingRows.Cast<AlternatingRow>().Last().BackColor;
            }
            else
            {
                Sheet.Rows[actualStartRowIndex, actualEndRowIndex].BackColor = Sheet.AlternatingRows.Cast<AlternatingRow>().First().BackColor;
            }
        }

        /// <summary>
        /// 変更された値をシートのセルへ反映します。
        /// </summary>
        /// <param name="row">値変更が行われた行オブジェクト。</param>
        /// <param name="pi">値変更が行われた行オブジェクトの PropertyInfo。</param>
        private void ReactiveCellValue(T row, PropertyInfo pi)
        {
            var attr = pi.GetCustomAttribute(typeof(MultiRowAttribute)) as MultiRowAttribute;
            if (attr == null)
            {
                return;
            }

            // 同一オブジェクトを保有する、最も早く出現する行オブジェクトを基準として値設定を行う
            var sheetRow = Sheet.RowHeader.Rows.Cast<Row>()
                .Where(x => object.Equals(x.Tag, row))
                .First();
            Sheet.Cells[sheetRow.Index + attr.Row, attr.Column].Value = pi.GetValue(row);
        }

        /// <summary>
        /// 画面の行インデックスから、アイテムを取得します。
        /// </summary>
        /// <param name="index">画面の行インデックス。</param>
        /// <returns>アイテム。</returns>
        public T GetItem(int index)
        {
            return (T)Sheet.RowHeader.Rows[index].Tag;
        }

        private Action<int, Cell> CellSetup { get; }

        /// <summary>
        /// 行を追加します。
        /// 外部からの利用は不要です。
        /// </summary>
        /// <param name="row">追加するアイテム。</param>
        /// <returns>追加されたアイテム</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public object AddRow(object row) => AddRow((T)row);

        /// <summary>
        /// 画面の行インデックスから、アイテムのインデックスを取得します。
        /// </summary>
        /// <param name="index">画面の行インデックス。</param>
        /// <returns>アイテムのインデックス。</returns>
        public int GetItemIndex(int index)
        {
            return (int)Math.Truncate(index / Convert.ToDecimal(RowsPerRecord));
        }

        /// <summary>
        /// 画面の行インデックスから、アイテムの MultiRowAttribute で指定された行インデックスを取得します。
        /// </summary>
        /// <param name="index">画面の行インデックス。</param>
        /// <returns>アイテムの MultiRowAttribute で指定された行インデックス。</returns>
        public int GetItemRowIndex(int index)
        {
            return index % RowsPerRecord;
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
    }
}