using FarPoint.Win.Spread;
using Metroit.ChangeTracking;
using Metroit.Collections.Generic;
using Metroit.Win.GcSpread.MultiRow.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Metroit.Win.GcSpread.MultiRow
{
    /// <summary>
    /// 1レコードを複数行として扱う機能を提供します。<br/>
    /// 行が追加されたとき、<see cref="Row.Tag"/>には行オブジェクトが設定されます。<br/>
    /// 必要なら<see cref="MultiRowSheetConfiguration{T}.TagDelivery"/>によってカスタマイズ可能です。
    /// </summary>
    /// <typeparam name="TRecord">状態を持つ変更追跡が可能なクラス。</typeparam>
    /// <remarks><see langword="null"/>が許容されるセルのときに、アイテムが<see langword="null"/>許容型でない場合、セルの見た目とアイテムの値が一致しない可能性があります。</remarks>
    public class MultiRowSheet<TRecord> : IDisposable where TRecord : IPropertyChangeTrackerProvider, IStateObject
    {
        /// <summary>
        /// 扱っているシートを取得します。
        /// </summary>
        public SheetView Sheet => Configuration.Sheet;

        /// <summary>
        /// 構成情報を取得します。
        /// </summary>
        public ReadOnlyMultiRowSheetConfiguration<TRecord> Configuration { get; }

        /// <summary>
        /// レコードのリストを取得します。
        /// </summary>
        public IReadOnlyList<TRecord> Rows => _list;

        /// <summary>
        /// 1レコードを複数行として扱う外観を提供します。
        /// </summary>
        private IMultiRowSheetAppearance MultiRowSheetAppearance { get; }

        /// <summary>
        /// 設定された構成情報。s
        /// </summary>
        private MultiRowSheetConfiguration<TRecord> _internalConfiguration { get; }

        /// <summary>
        /// 1レコードを複数行として扱うリスト。
        /// </summary>
        private TrackingList<TRecord> _list;

        /// <summary>
        /// 構成情報によって<see cref="SheetView"/>を1レコード複数行の表現で扱えるようにします。<br/>
        /// 対象としたシートの<see cref="SheetView.OperationMode"/>は<see cref="OperationMode.Normal"/>に設定されます。<br/>
        /// <see cref="FpSpread.LegacyBehaviors"/>に<see cref="LegacyBehaviors.Style"/>が含まれるとき、<see cref="Rows.DefaultRow"/>の<see cref="Row.Resizable"/>が<see langword="false"/>に設定されます。<br/>
        /// <see cref="FpSpread.LegacyBehaviors"/>に<see cref="LegacyBehaviors.Style"/>が含まれないとき、追加された行の<see cref="Row.Resizable"/>が<see langword="false"/>に設定されます。
        /// </summary>
        /// <param name="configuration">構成情報。</param>
        /// <param name="list">取り扱うリスト。</param>
        public static MultiRowSheet<TRecord> Start(MultiRowSheetConfiguration<TRecord> configuration, TrackingList<TRecord> list)
        {
            return new MultiRowSheet<TRecord>(configuration, list);
        }

        /// <summary>
        /// 新しいインスタンスを生成します。<br/>
        /// 対象としたシートの<see cref="SheetView.OperationMode"/>は<see cref="OperationMode.Normal"/>に設定されます。<br/>
        /// <see cref="FpSpread.LegacyBehaviors"/>に<see cref="LegacyBehaviors.Style"/>が含まれる場合、<see cref="Rows.DefaultRow"/>の<see cref="Row.Resizable"/>は<see langword="false"/>に設定されます。<br/>
        /// </summary>
        /// <param name="configuration">構成情報。</param>
        /// <param name="list">取り扱うリスト。</param>
        private MultiRowSheet(MultiRowSheetConfiguration<TRecord> configuration, TrackingList<TRecord> list)
        {
            _internalConfiguration = configuration;
            Configuration = new ReadOnlyMultiRowSheetConfiguration<TRecord>(configuration);

            Sheet.OperationMode = OperationMode.Normal;

            AttachSheetViewEvents();

            if ((Sheet.FpSpread.LegacyBehaviors & LegacyBehaviors.Style) == LegacyBehaviors.Style)
            {
                MultiRowSheetAppearance = new StyledRowAppearance(Sheet);
                MultiRowSheetAppearance.DisableRowResize(null);
            }
            else
            {
                MultiRowSheetAppearance = new UnstyledRowAppearance(Sheet);
            }
            MultiRowSheetAppearance.SetAlternatingRowsBackColor(Configuration.RowsPerRecord,
                Configuration.OddBackColor, Configuration.EvenBackColor);

            _list = list;
            _list.ListChanged += TrackingList_ListChanged;
        }

        /// <summary>
        /// SheetView に必要なイベントをアタッチします。
        /// </summary>
        private void AttachSheetViewEvents()
        {
            Sheet.FpSpread.Enter += FpSpread_Enter;
            Sheet.FpSpread.Leave += FpSpread_Leave;
            Sheet.FpSpread.ActiveSheetChanged += FpSpread_ActiveSheetChanged;
            Sheet.FpSpread.CellClick += FpSpread_CellClick;
            Sheet.FpSpread.MouseMove += FpSpread_MouseMove;
            Sheet.FpSpread.MouseUp += FpSpread_MouseUp;
            Sheet.FpSpread.RowDragMoveCompleted += FpSpread_RowDragMoveCompleted;
            Sheet.CellChanged += Sheet_CellChanged;
        }

        /// <summary>
        /// 画面の行インデックスから画面の開始行インデックスを取得します。
        /// </summary>
        /// <returns>画面の開始行インデックス。</returns>
        public int GetViewStartRowIndexFromViewRowIndex(int viewRowIndex)
        {
            var record = GetRowTag(Sheet.RowHeader.Rows[viewRowIndex]);
            return GetViewStartRowIndexFromRecord(record);
        }

        /// <summary>
        /// レコードのインデックスから画面の開始行インデックスを取得します。
        /// </summary>
        /// <returns>画面の開始行インデックス。</returns>
        public int GetViewStartRowIndexFromRecordIndex(int recordIndex)
        {
            return GetViewStartRowIndexFromRecord(Rows[recordIndex]);
        }

        /// <summary>
        /// レコードから画面の開始行の行ヘッダーを取得します。
        /// </summary>
        /// <param name="record">レコード。</param>
        /// <returns>画面の開始行の行ヘッダー。</returns>
        public Row GetViewStartRowFromRecord(TRecord record)
        {
            return Sheet.RowHeader.Rows
                .OfType<Row>()
                .First(x => EqualityComparer<object>.Default.Equals(GetRowTag(x), record));
        }

        /// <summary>
        /// レコードから画面の開始行インデックスを取得します。
        /// </summary>
        /// <param name="record">レコード。</param>
        /// <returns>画面の開始行インデックス。</returns>
        public int GetViewStartRowIndexFromRecord(TRecord record)
        {
            return GetViewStartRowFromRecord(record).Index;
        }

        /// <summary>
        /// 画面の行インデックスから、レコードのインデックスを取得します。
        /// </summary>
        /// <param name="viewRowIndex">画面の行インデックス。</param>
        /// <returns>レコードのインデックス。</returns>
        public int GetRecordIndex(int viewRowIndex)
        {
            return _list.IndexOf(GetRecord(viewRowIndex));
        }

        /// <summary>
        /// 画面の行インデックスからレコードを取得します。
        /// </summary>
        /// <param name="viewRowIndex">画面の行インデックス。</param>
        /// <returns>レコード。</returns>
        public TRecord GetRecord(int viewRowIndex)
        {
            return GetRowTag(Sheet.RowHeader.Rows[viewRowIndex]);
        }

        /// <summary>
        /// 画面の行インデックスから行番号を取得します。
        /// </summary>
        /// <param name="viewRowIndex">画面の行インデックス。</param>
        /// <returns>行番号。</returns>
        public int GetRowNumber(int viewRowIndex)
        {
            return Configuration.GetRowNumber(viewRowIndex);
        }

        /// <summary>
        /// 画面の行インデックスから、<see cref="MultiRowAttribute"/> で指定されている行インデックスを取得します。
        /// </summary>
        /// <param name="viewRowIndex">画面の行インデックス。</param>
        /// <returns><see cref="MultiRowAttribute"/> で指定されている行インデックス。</returns>
        public int GetAttributeRowIndex(int viewRowIndex)
        {
            return Configuration.GetAttributeRowIndex(viewRowIndex);
        }

        /// <summary>
        /// 行番号から、行を表現するアルファベットに変換します。
        /// </summary>
        /// <param name="rowNumber">行番号。</param>
        /// <returns>行を表現するアルファベット。</returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        private static string RowNumberToLetters(int rowNumber)
        {
            if (rowNumber <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(rowNumber), "Must be 1 or greater.");
            }

            var result = string.Empty;
            while (rowNumber > 0)
            {
                rowNumber--;
                result = (char)('A' + rowNumber % 26) + result;
                rowNumber /= 26;
            }
            return result;
        }

        /// <summary>
        /// クリックによってマウスダウンされた位置。
        /// </summary>
        private Point _mouseDownPoint;

        /// <summary>
        /// 行をドラッグ中かどうか。
        /// </summary>
        private bool _isRowDragging = false;

        /// <summary>
        /// クリックによってマウスダウンされた実際の行インデックス。
        /// </summary>
        private int _mouseDownRow = -1;

        /// <summary>
        /// ドラッグと判定する移動距離の閾値。
        /// </summary>
        private readonly int DragThreshold = 5;

        /// <summary>
        /// マウスアップで行移動を完了させる。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FpSpread_MouseUp(object sender, MouseEventArgs e)
        {
            if (!IsActive)
            {
                return;
            }

            if (e.Button != MouseButtons.Left)
            {
                return;
            }

            // 行移動させたときには状態をリセット
            if (_isRowDragging)
            {
                _mouseDownRow = -1;
                _isRowDragging = false;
            }
        }

        /// <summary>
        /// 画面の開始行インデックスに該当する画面の行を選択状態にします。
        /// </summary>
        /// <param name="viewStartRowIndex">画面の開始行インデックス。</param>
        private void SelectViewRows(int viewStartRowIndex)
        {
            Sheet.SetActiveCell(viewStartRowIndex, -1, true);
            Sheet.AddSelection(viewStartRowIndex, -1, Configuration.RowsPerRecord, -1);
        }

        /// <summary>
        /// マウス移動時の処理。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FpSpread_MouseMove(object sender, MouseEventArgs e)
        {
            if (!IsActive)
            {
                return;
            }

            if (e.Button != MouseButtons.Left || _mouseDownRow < 0)
            {
                return;
            }

            // 行ヘッダー以外はスキップ
            var hitTest = Sheet.FpSpread.HitTest(e.X, e.Y);
            if (hitTest?.HeaderInfo == null)
            {
                return;
            }
            if (hitTest.Type != HitTestType.RowHeader)
            {
                return;
            }

            // ドラッグ開始判定
            int deltaX = Math.Abs(e.X - _mouseDownPoint.X);
            int deltaY = Math.Abs(e.Y - _mouseDownPoint.Y);

            if (!_isRowDragging && (deltaX > DragThreshold || deltaY > DragThreshold))
            {
                _isRowDragging = true;
            }
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

            _actionBeginOperation = ActionBeginOperation.ViewCell;
            var item = GetRecord(e.Row);

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
        /// MultiRowSheet の操作前の AllowRowMoveMultiple プロパティ。
        /// </summary>
        private bool _originalAllowRowMoveMultiple = false;

        /// <summary>
        /// フォーカスが当たったとき、対象シートであれば AllowRowMoveMultiple を有効にする。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FpSpread_Enter(object sender, EventArgs e)
        {
            if (!IsActive)
            {
                return;
            }

            EnableAllowRowMoveMultiple();
        }

        /// <summary>
        /// フォーカスが外れたとき、対象シートであれば AllowRowMoveMultiple を元に戻す。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FpSpread_Leave(object sender, EventArgs e)
        {
            if (!IsActive)
            {
                return;
            }

            DisableAllowRowMoveMultiple();
        }

        /// <summary>
        /// アクティブなシートが変更されたとき、対象シートであれば AllowRowMoveMultiple を有効にし、異なれば元に戻す。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FpSpread_ActiveSheetChanged(object sender, EventArgs e)
        {
            if (IsActive)
            {
                EnableAllowRowMoveMultiple();
            }
            else
            {
                DisableAllowRowMoveMultiple();
            }
        }

        /// <summary>
        /// AllowRowMoveMultiple を有効にする。
        /// </summary>
        private void EnableAllowRowMoveMultiple()
        {
            _originalAllowRowMoveMultiple = Sheet.FpSpread.AllowRowMoveMultiple;
            Sheet.FpSpread.AllowRowMoveMultiple = true;
        }

        /// <summary>
        /// AllowRowMoveMultiple を元に戻す。
        /// </summary>
        private void DisableAllowRowMoveMultiple()
        {
            Sheet.FpSpread.AllowRowMoveMultiple = _originalAllowRowMoveMultiple;
        }

        /// <summary>
        /// 行ヘッダーをクリックしたとき、1レコードのすべての行を選択状態にする。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FpSpread_CellClick(object sender, CellClickEventArgs e)
        {
            if (!IsActive)
            {
                return;
            }

            if (e.Button != MouseButtons.Left)
            {
                return;
            }

            HitTestInformation hitTest = Sheet.FpSpread.HitTest(e.X, e.Y);

            // 行ヘッダー以外はスキップ
            if (hitTest.HeaderInfo == null)
            {
                return;
            }
            if (hitTest.Type != HitTestType.RowHeader)
            {
                return;
            }

            _mouseDownPoint = Sheet.FpSpread.PointToClient(Cursor.Position);
            _mouseDownRow = hitTest.HeaderInfo.Row;
            _isRowDragging = false;

            var viewStartRowIndex = GetViewStartRowIndexFromViewRowIndex(_mouseDownRow);

            var delayedSelectRecordTask = new Task(() =>
            {
                if (Sheet.FpSpread.InvokeRequired)
                {
                    Sheet.FpSpread.Invoke(new System.Action(() =>
                    {
                        SelectViewRows(viewStartRowIndex);
                    }));
                }
                else
                {
                    SelectViewRows(viewStartRowIndex);
                }
            });


            // NOTE: Shift/Ctrlキーが押されている場合は、標準動作のあとに1レコードを選択状態とする。
            if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift ||
                (Control.ModifierKeys & Keys.Control) == Keys.Control)
            {
                delayedSelectRecordTask.Start();
                return;
            }

            // NOTE: 移動が許容されていないときは標準動作によって1レコード全体が選択状態とならないため、標準動作のあとに1レコード全体を選択状態とする。
            if (!Sheet.FpSpread.AllowRowMove)
            {
                delayedSelectRecordTask.Start();
                return;
            }

            // 通常のクリック：1レコードを選択
            SelectViewRows(viewStartRowIndex);
        }

        /// <summary>
        /// 行のドラッグ移動が完了したとき、ドラッグした行と下の行番号を再描画する。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FpSpread_RowDragMoveCompleted(object sender, DragMoveCompletedEventArgs e)
        {
            if (!IsActive)
            {
                return;
            }

            var fromIndex = e.FromIndex;
            var toIndex = e.ToIndex;

            if (e.ToIndex < e.FromIndex)
            {
                fromIndex = e.ToIndex;
                toIndex = e.FromIndex;
            }

            ReDrawRowAppearance(fromIndex, toIndex);
        }

        /// <summary>
        /// リストの内容が変更されたときに走行する。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TrackingList_ListChanged(object sender, ListChangedEventArgs e)
        {
            switch (e.ListChangedType)
            {
                case ListChangedType.ItemAdded:
                    // Add(), AddNew(), Insert() で走行する
                    ReactiveAddedRow(e.NewIndex);
                    break;

                case ListChangedType.ItemChanged:
                    // ResetItem(), INotifyPropertyChangedによって値変更が通知されたときに走行する
                    ReactiveChangedRow(e.NewIndex, e.PropertyDescriptor.Name);
                    break;

                case ListChangedType.Reset:
                    // Clear() で走行する
                    ClearViewRows();
                    break;

                case ListChangedType.ItemDeleted:
                    // Remove(), RemoveAt(), CancelNew() で走行する
                    RemoveViewRow();
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

            AddViewRow(_list[rowIndex]);

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
        private void AddViewRow(TRecord item)
        {
            var addingViewStartRowIndex = Sheet.Rows.Count;
            Sheet.Rows.Add(addingViewStartRowIndex, Configuration.RowsPerRecord);
            var addedRows = new List<Row>();
            for (var i = addingViewStartRowIndex; i < addingViewStartRowIndex + Configuration.RowsPerRecord; i++)
            {
                addedRows.Add(Sheet.Rows[i]);
            }

            // LegacyBehaviors.Style が設定されていない場合は、行のリサイズを無効化する
            if ((Sheet.FpSpread.LegacyBehaviors & LegacyBehaviors.Style) == 0)
            {
                MultiRowSheetAppearance.DisableRowResize(addedRows);
            }
            if (Configuration.RowSetup != null)
            {
                Configuration.RowSetup.Invoke(addingViewStartRowIndex, addedRows);
            }

            // 行ヘッダーの結合、行番号の割当と行の背景色を変更する
            DrawRowAppearance(addingViewStartRowIndex);

            // 追加されたすべての行の Tag に、行オブジェクトを設定し、セルセットアップを実施する
            var viewEndRowIndex = Sheet.Rows.Count - 1;
            for (var viewRowIndex = addingViewStartRowIndex; viewRowIndex <= viewEndRowIndex; viewRowIndex++)
            {
                SetRowTag(Sheet.RowHeader.Rows[viewRowIndex], item);

                if (Configuration.CellSetup == null)
                {
                    continue;
                }

                for (var columnIndex = 0; columnIndex < Sheet.Columns.Count; columnIndex++)
                {
                    Configuration.CellSetup.Invoke(GetAttributeRowIndex(viewRowIndex), Sheet.Cells[viewRowIndex, columnIndex]);
                }
            }

            _internalConfiguration.NotifyRowAdded(new RowAddedEventArgs(addingViewStartRowIndex, addedRows));
        }

        /// <summary>
        /// 行オブジェクトのタグを設定する。
        /// </summary>
        /// <param name="row">行オブジェクト。</param>
        /// <param name="item">タグへの設定情報。</param>
        private void SetRowTag(Row row, TRecord item)
        {
            if (Configuration.TagDelivery == null)
            {
                row.Tag = item;
                return;
            }

            Configuration.TagDelivery.SetTag.Invoke(row, item);
        }

        /// <summary>
        /// 行オブジェクトのタグを取得する。
        /// </summary>
        /// <param name="row">行オブジェクト。</param>
        /// <returns>タグの設定情報。</returns>
        private TRecord GetRowTag(Row row)
        {
            if (Configuration.TagDelivery == null)
            {
                return (TRecord)row.Tag;
            }

            return Configuration.TagDelivery.GetTag.Invoke(row.Tag);
        }

        /// <summary>
        /// 変更された行をリアクティブする。
        /// </summary>
        /// <param name="rowIndex">変更された行。</param>
        /// <param name="propertyName">変更があったプロパティ名。</param>
        private void ReactiveChangedRow(int rowIndex, string propertyName)
        {
            // NOTE: Sheet_CellChanged から制御が移ったときは行わない
            if (_actionBeginOperation == ActionBeginOperation.None)
            {
                _actionBeginOperation = ActionBeginOperation.Item;

                // 変更されたプロパティの値を実際のセルへ反映する
                var pi = _list[rowIndex].GetType()
                    .GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty)
                    .Single(x => x.Name == propertyName);
                ReactiveCellValue(_list[rowIndex], pi);
            }

            _actionBeginOperation = ActionBeginOperation.None;
        }

        /// <summary>
        /// 変更された値をシートのセルへ反映します。
        /// </summary>
        /// <param name="record">値変更が行われた行オブジェクト。</param>
        /// <param name="pi">値変更が行われた行オブジェクトの PropertyInfo。</param>
        private void ReactiveCellValue(TRecord record, PropertyInfo pi)
        {
            var attr = pi.GetCustomAttribute(typeof(MultiRowAttribute)) as MultiRowAttribute;
            if (attr == null)
            {
                return;
            }

            // 同一オブジェクトを保有する、最も早く出現する行オブジェクトを基準として値設定を行う
            var viewRow = GetViewStartRowFromRecord(record);
            Sheet.Cells[viewRow.Index + attr.Row, attr.Column].Value = pi.GetValue(record);
        }

        /// <summary>
        /// 実際の行をすべて削除する。
        /// </summary>
        private void ClearViewRows()
        {
            Sheet.Rows.Clear();
        }

        /// <summary>
        /// 画面の行を削除する。
        /// </summary>
        private void RemoveViewRow()
        {
            var viewStartRowIndex = GetViewStartRowIndexFromRecord(_list.LastAccessItem);
            Sheet.Rows.Remove(viewStartRowIndex, Configuration.RowsPerRecord);
            ReDrawRowAppearance(viewStartRowIndex);

            _internalConfiguration.NotifyRowRemoved(new RowRemovedEventArgs(viewStartRowIndex, viewStartRowIndex + Configuration.RowsPerRecord - 1));
        }

        /// <summary>
        /// <paramref name="viewStartRowIndex"/> を含む下の行を描画し直す。
        /// </summary>
        /// <param name="viewStartRowIndex">画面の開始行インデックス。</param>
        private void ReDrawRowAppearance(int viewStartRowIndex)
        {
            foreach (var row in Sheet.Rows
                .OfType<Row>()
                .Select((Row, Index) => new { Row, Index })
                .Where(x => x.Index >= viewStartRowIndex && GetAttributeRowIndex(x.Index) == 0))
            {
                DrawRowAppearance(row.Index);
            }
        }

        /// <summary>
        /// <paramref name="viewStartRowIndex"/> から <paramref name="viewEndRowIndex"/> に含まれる行を描画し直す。
        /// </summary>
        /// <param name="viewStartRowIndex">画面の開始行インデックス。</param>
        /// <param name="viewEndRowIndex">画面の終了行インデックス。</param>
        private void ReDrawRowAppearance(int viewStartRowIndex, int viewEndRowIndex)
        {
            foreach (var row in Sheet.Rows
                .OfType<Row>()
                .Select((Row, Index) => new { Row, Index })
                .Where(x => x.Index >= viewStartRowIndex && x.Index <= viewEndRowIndex && GetAttributeRowIndex(x.Index) == 0))
            {
                DrawRowAppearance(row.Index);
            }
        }

        /// <summary>
        /// 行ヘッダーのセル結合、行番号の設定、背景色の設定を行う。
        /// </summary>
        /// <param name="viewStartRowIndex">画面の開始行インデックス。</param>
        private void DrawRowAppearance(int viewStartRowIndex)
        {
            MergeRowNumberCell(viewStartRowIndex);
            SetRowNumber(viewStartRowIndex);
        }

        /// <summary>
        /// 行番号を表現する行ヘッダーセルを結合する。
        /// </summary>
        /// <param name="viewStartRowIndex">画面の開始行インデックス。</param>
        private void MergeRowNumberCell(int viewStartRowIndex)
        {
            Sheet.AddRowHeaderSpanCell(viewStartRowIndex, GetAutoTextIndex(), Configuration.RowsPerRecord, 1);
        }

        /// <summary>
        /// 1レコードの行ヘッダーに表示する行番号を設定する。
        /// </summary>
        /// <param name="viewStartRowIndex">画面の開始行インデックス。</param>
        private void SetRowNumber(int viewStartRowIndex)
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

            var rowNumber = GetRowNumber(viewStartRowIndex);
            var viewEndRowIndex = GetViewEndRowIndex(viewStartRowIndex);

            var rowNumberColumnIndex = GetAutoTextIndex();
            for (var i = viewStartRowIndex; i <= viewEndRowIndex; i++)
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
        /// 画面の開始行インデックスから、画面の終了行インデックスを取得します。
        /// </summary>
        /// <param name="viewStartRowIndex">画面の開始行インデックス。</param>
        /// <returns></returns>
        private int GetViewEndRowIndex(int viewStartRowIndex)
        {
            return Configuration.GetViewEndRowIndex(viewStartRowIndex);
        }

        /// <summary>
        /// 行番号を表現する列のインデックスを取得します。
        /// </summary>
        /// <returns>行番号を表現する列のインデックス。</returns>
        private int GetAutoTextIndex()
        {
            var index = Sheet.RowHeader.AutoTextIndex;
            if (index == -1)
            {
                index = Sheet.RowHeader.ColumnCount - 1;
            }

            return index;
        }

        /// <summary>
        /// シートがアクティブかどうかを取得します。
        /// </summary>
        /// <returns>シートがアクティブなら<see langword="true"/>, それ以外は<see langword="false"/>を返却します。</returns>
        public bool IsActive => Sheet.FpSpread.ActiveSheet == Sheet;

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
                DetachSheetViewEvents();
            }

            disposed = true;
        }

        /// <summary>
        /// SheetView から必要なイベントをデタッチします。
        /// </summary>
        private void DetachSheetViewEvents()
        {
            Sheet.FpSpread.Enter -= FpSpread_Enter;
            Sheet.FpSpread.Leave -= FpSpread_Leave;
            Sheet.FpSpread.ActiveSheetChanged -= FpSpread_ActiveSheetChanged;
            Sheet.FpSpread.CellClick -= FpSpread_CellClick;
            Sheet.FpSpread.MouseMove -= FpSpread_MouseMove;
            Sheet.FpSpread.MouseUp -= FpSpread_MouseUp;
            Sheet.FpSpread.RowDragMoveCompleted -= FpSpread_RowDragMoveCompleted;
            Sheet.CellChanged -= Sheet_CellChanged;
        }
    }
}