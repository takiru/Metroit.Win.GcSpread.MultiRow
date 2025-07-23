using FarPoint.Win.Spread;
using Metroit.Collections.Generic;
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
    public class MultiRowSheet<T> : IDisposable where T : IPropertyChangeTracker, IStateObject, new()
    {
        /// <summary>
        /// 扱っているシートを取得します。
        /// </summary>
        public SheetView Sheet { get; protected set; }

        /// <summary>
        /// 1レコードに対する行数を取得します。
        /// </summary>
        public int RowsPerRecord { get; protected set; }

        /// <summary>
        /// 1レコードを複数行として扱うリスト。
        /// </summary>
        private TrackingList<T> _list;

        /// <summary>
        /// 行のコレクションを取得します。
        /// </summary>
        public IReadOnlyList<T> Rows => _list;

        /// <summary>
        /// 行の追加が行われた時、セルの CellType や Tag の設定, セル結合などのセットアップを行うデリゲート。
        /// </summary>
        private Action<int, Cell> CellSetup { get; }

        /// <summary>
        /// 新しいインスタンスを生成します。
        /// </summary>
        /// <param name="sheet">シートオブジェクト。</param>
        /// <param name="list">取り扱うリスト。</param>
        /// <param name="rowsPerRecord">1レコードの行数。</param>
        /// <param name="cellSetup">行の追加が行われた時、セルの CellType や Tag の設定, セル結合などを行い、行のセル情報設定します。</param>
        public MultiRowSheet(SheetView sheet, int rowsPerRecord, TrackingList<T> list, Action<int, Cell> cellSetup = null)
        {
            Sheet = sheet;
            Sheet.CellChanged += Sheet_CellChanged;

            RowsPerRecord = rowsPerRecord;
            _list = list;
            _list.ListChanged += _list_ListChanged;

            CellSetup = cellSetup;
        }

        /// <summary>
        /// 実際の行インデックスからアイテムを取得します。
        /// </summary>
        /// <param name="actualRowIndex">実際の行インデックス。</param>
        /// <returns>アイテム。</returns>
        public T GetItem(int actualRowIndex)
        {
            return (T)Sheet.RowHeader.Rows[actualRowIndex].Tag;
        }

        /// <summary>
        /// 実際の行インデックスから、アイテムのインデックスを取得します。
        /// </summary>
        /// <param name="actualRowIndex">実際の行インデックス。</param>
        /// <returns>アイテムのインデックス。</returns>
        public int GetItemIndex(int actualRowIndex)
        {
            var item = (T)Sheet.RowHeader.Rows[actualRowIndex].Tag;
            return _list.IndexOf(item);
        }

        /// <summary>
        /// 実際の行インデックスから、MultiRowAttribute で指定された行インデックスを取得します。
        /// </summary>
        /// <param name="actualRowIndex">実際の行インデックス。</param>
        /// <returns>MultiRowAttribute で指定された行インデックス。</returns>
        public int GetAttributeRowIndex(int actualRowIndex)
        {
            return actualRowIndex % RowsPerRecord;
        }

        /// <summary>
        /// どのような操作によってアクションが行われたか。
        /// </summary>
        private ActionBeginOperation _actionBeginOperation = ActionBeginOperation.None;

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
                    if (attr.Row != GetAttributeRowIndex(e.Row) || attr.Column != e.Column)
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
        /// リストの内容が変更されたときに走行する。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _list_ListChanged(object sender, ListChangedEventArgs e)
        {
            switch (e.ListChangedType)
            {
                case ListChangedType.ItemAdded:
                    // Add(), AddNew(), Insert() で走行する
                    ReactiveAddedRow(e.NewIndex);
                    break;

                case ListChangedType.ItemChanged:
                    // ResetItem(), INotifyPropertyChangedによって値変更が通知されたときに走行する
                    ReactiveChangedRow(e.NewIndex);
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
        /// 追加された行をリアクティブにする。
        /// </summary>
        /// <param name="rowIndex">追加された行。</param>
        private void ReactiveAddedRow(int rowIndex)
        {
            _actionBeginOperation = ActionBeginOperation.Item;

            // 追跡されていない場合は、追跡を開始する
            if (!_list.Last().ChangeTracker.IsTracking)
            {
                _list.Last().ChangeTracker.Reset();
            }

            AddActualRow(_list[rowIndex]);

            // アイテムの値を実際のセルへ反映する
            var pis = _list[rowIndex].GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty);
            foreach (var pi in pis)
            {
                ReactiveCellValue(_list[rowIndex], pi);
            }

            _actionBeginOperation = ActionBeginOperation.None;
        }

        /// <summary>
        /// シートに実際の行を追加する。
        /// </summary>
        /// <param name="item">追加された行のアイテム。</param>
        private void AddActualRow(T item)
        {
            Sheet.Rows.Add(Sheet.Rows.Count, RowsPerRecord);

            var actualStartRowIndex = Sheet.Rows.Count - RowsPerRecord;

            // 追加されたすべての行の Tag に、行オブジェクトを設定し、セルセットアップを実施する
            var actualEndRowIndex = Sheet.Rows.Count - 1;
            for (var actualRowIndex = actualStartRowIndex; actualRowIndex <= actualEndRowIndex; actualRowIndex++)
            {
                Sheet.RowHeader.Rows[actualRowIndex].Tag = item;
                if (CellSetup == null)
                {
                    continue;
                }

                for (var columnIndex = 0; columnIndex < Sheet.Columns.Count; columnIndex++)
                {
                    CellSetup.Invoke(GetAttributeRowIndex(actualRowIndex), Sheet.Cells[actualRowIndex, columnIndex]);
                }
            }

            // 行番号の割当と行の背景色を変更する
            DrawRowStyle(actualStartRowIndex);
        }

        /// <summary>
        /// 変更された行をリアクティブする。
        /// </summary>
        /// <param name="rowIndex">変更された行。</param>
        private void ReactiveChangedRow(int rowIndex)
        {
            // NOTE: Sheet_CellChanged から制御が移ったときは行わない
            if (_actionBeginOperation == ActionBeginOperation.None)
            {
                _actionBeginOperation = ActionBeginOperation.Item;

                // 直前で変更されたプロパティの値を実際のセルへ反映する
                var pi = _list[rowIndex].GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty)
                    .Where(x => x.Name == (_list[rowIndex]).ChangeTracker.LastTrackingProperty)
                    .Single();
                ReactiveCellValue(_list[rowIndex], pi);
            }

            _actionBeginOperation = ActionBeginOperation.None;
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
        /// 実際の行をすべて削除する。
        /// </summary>
        private void ClearActualRows()
        {
            Sheet.Rows.Clear();
        }

        /// <summary>
        /// 実際の行を削除する。
        /// </summary>
        private void RemoveActualRow()
        {
            var actualStartRowIndex = Sheet.RowHeader.Rows.Cast<Row>()
                .Select((Value, Index) => new { Index, Value })
                .Where(x => x.Value.Tag == (object)_list.LastAccessItem)
                .First()
                .Index;
            Sheet.Rows.Remove(actualStartRowIndex, RowsPerRecord);
            ReDrawBelowRows(actualStartRowIndex);
        }

        /// <summary>
        /// <paramref name="actualStartRowIndex"/> を含む下の行を描画し直す。
        /// </summary>
        /// <param name="actualStartRowIndex">1レコードの実際の開始行インデックス。</param>
        private void ReDrawBelowRows(int actualStartRowIndex)
        {
            foreach (var row in Sheet.Rows.Cast<Row>()
                .Select((Row, Index) => new { Row, Index })
                .Where(x => x.Index >= actualStartRowIndex && GetAttributeRowIndex(x.Index) == 0))
            {
                SetRowNumber(row.Index);
                ChangeBackgroundColor(row.Index);
            }
        }

        /// <summary>
        /// 行ヘッダーのセル結合、行番号の設定、背景色の設定を行う。
        /// </summary>
        /// <param name="actualStartRowIndex">1レコードの実際の開始行インデックス。</param>
        private void DrawRowStyle(int actualStartRowIndex)
        {
            MergeRowNumberCell(actualStartRowIndex);
            SetRowNumber(actualStartRowIndex);
            ChangeBackgroundColor(actualStartRowIndex);
        }

        /// <summary>
        /// 行番号を表現する行ヘッダーセルを結合する。
        /// </summary>
        /// <param name="actualStartRowIndex">1レコードの実際の開始行インデックス。</param>
        private void MergeRowNumberCell(int actualStartRowIndex)
        {
            Sheet.AddRowHeaderSpanCell(actualStartRowIndex, GetRowNumberColumnIndex(), RowsPerRecord, 1);
        }

        /// <summary>
        /// 1レコードの行ヘッダーに表示する行番号を設定する。
        /// </summary>
        /// <param name="actualStartRowIndex">1レコードの実際の開始行インデックス。</param>
        private void SetRowNumber(int actualStartRowIndex)
        {
            // 行ヘッダーがない場合は何もしない
            if (Sheet.RowHeader.Columns.Count == 0)
            {
                return;
            }

            // 行ヘッダーの自動テキストが空の場合は何もしない
            if (Sheet.RowHeader.AutoText == HeaderAutoText.Blank)
            {
                return;
            }

            var rowNumber = GetItemIndex(actualStartRowIndex) + 1;
            var actualEndRowIndex = actualStartRowIndex + RowsPerRecord - 1;

            var rowNumberColumnIndex = GetRowNumberColumnIndex();
            for (var i = actualStartRowIndex; i <= actualEndRowIndex; i++)
            {
                if (Sheet.RowHeader.AutoText == HeaderAutoText.Numbers)
                {
                    Sheet.RowHeader.Cells[i, rowNumberColumnIndex].Value = rowNumber;
                    continue;
                }
                if (Sheet.RowHeader.AutoText == HeaderAutoText.Letters)
                {
                    Sheet.RowHeader.Cells[i, rowNumberColumnIndex].Value = RowNumberToLetters(rowNumber);
                }
            }
        }

        /// <summary>
        /// 行番号を表現する列のインデックスを取得します。
        /// </summary>
        /// <returns>行番号を表現する列のインデックス。</returns>
        private int GetRowNumberColumnIndex()
        {
            var rowNumberColumn = Sheet.RowHeader.AutoTextIndex;
            if (rowNumberColumn == -1)
            {
                rowNumberColumn = 0;
            }

            return rowNumberColumn;
        }

        /// <summary>
        /// 行番号を数値からアルファベットに変換します。
        /// </summary>
        /// <param name="number">行番号。</param>
        /// <returns>アルファベット。</returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        private string RowNumberToLetters(int number)
        {
            if (number <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(number), "1以上である必要があります。");
            }

            var result = string.Empty;
            while (number > 0)
            {
                number--;
                result = (char)('A' + (number % 26)) + result;
                number /= 26;
            }
            return result;
        }

        /// <summary>
        /// 1レコードの行の背景色を変更する。
        /// </summary>
        /// <param name="actualStartRowIndex">1レコードの実際の開始行インデックス。</param>
        private void ChangeBackgroundColor(int actualStartRowIndex)
        {
            if (Sheet.AlternatingRows.Count == 0)
            {
                return;
            }

            // 行が対象となる AlternatingRow を求める
            var rowNumber = GetItemIndex(actualStartRowIndex);
            var alternatingRowIndex = rowNumber % RowsPerRecord;

            var alternatingRow = Sheet.AlternatingRows.Cast<AlternatingRow>()
                .Select((Value, Index) => new { Index, Value })
                .Where(x => x.Index == alternatingRowIndex)
                .FirstOrDefault();
            if (alternatingRow == null)
            {
                return;
            }

            var actualEndRowIndex = actualStartRowIndex + RowsPerRecord - 1;
            Sheet.Rows[actualStartRowIndex, actualEndRowIndex].BackColor = alternatingRow.Value.BackColor;
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