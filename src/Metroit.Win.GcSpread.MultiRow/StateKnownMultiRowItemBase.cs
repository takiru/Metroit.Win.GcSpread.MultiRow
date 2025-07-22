using FarPoint.Win.Spread;
using System.Linq;
using System.Reflection;

namespace Metroit.Win.GcSpread.MultiRow
{
    /// <summary>
    /// 値の変更状態を有する1レコード複数行のアイテムを提供します。
    /// </summary>
    public abstract class StateKnownMultiRowItemBase : IStateObject
    {
        /// <summary>
        /// 新しいインスタンスを生成します。
        /// </summary>
        public StateKnownMultiRowItemBase()
        {
        }

        /// <summary>
        /// アイテムが認識しているシートを取得します。
        /// </summary>
        public SheetView Sheet { get; private set; }

        /// <summary>
        /// シートの操作状態を取得します。
        /// </summary>
        internal ISheetState SheetState { get; private set; }

        /// <summary>
        /// アイテムプロパティの値変更を行ったかどうかを取得します。
        /// </summary>
        internal bool IsItemValueChanged { get; private set; } = false;

        private ItemState _state = ItemState.New;

        public ItemState State => _state;

        public void ChangeState(ItemState state)
        {
            _state = state;
        }

        /// <summary>
        /// アイテムの初期化を行います。
        /// </summary>
        /// <param name="sheet">シートオブジェクト。</param>
        /// <param name="sheetState">シートの操作状態オブジェクト。</param>
        internal void Initialize(SheetView sheet, ISheetState sheetState)
        {
            Sheet = sheet;
            SheetState = sheetState;
        }

        ///// <summary>
        ///// 変更された値を通知します。
        ///// </summary>
        ///// <typeparam name="T">設定するプロパティ情報</typeparam>
        ///// <param name="propertyName">プロパティ名。</param>
        ///// <param name="value">値。</param>
        //protected override void NotifyChangedValue<T>(string propertyName, T value)
        //{
        //    // リアクティブ可能な時のみリアクティブする
        //    if (!CanReactive())
        //    {
        //        return;
        //    }

        //    ReactiveCellValue(propertyName, value);
        //}

        /// <summary>
        /// 変更された値をシートのセルへ反映します。
        /// </summary>
        /// <typeparam name="T">設定するプロパティ情報</typeparam>
        /// <param name="propertyName">プロパティ名。</param>
        /// <param name="value">値。</param>
        internal void ReactiveCellValue<T>(string propertyName, T value)
        {
            // シートへの値を反映
            var pi = GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty);
            if (pi == null)
            {
                return;
            }

            var attr = pi.GetCustomAttribute(typeof(MultiRowAttribute)) as MultiRowAttribute;
            if (attr == null)
            {
                return;
            }

            IsItemValueChanged = true;

            // 自身のオブジェクトを把握している視覚的な行インデックスを求める
            // NOTE: 予め追加された時の行インデックスを把握していても、行削除が行われるとインデックスを見失うため。
            var row = Sheet.RowHeader.Rows.Cast<Row>().Where(x => x.Tag == this).First();
            Sheet.Cells[row.Index + attr.Row, attr.Column].Value = value;

            IsItemValueChanged = false;
        }

        /// <summary>
        /// リアクティブ可能かどうかを取得する。
        /// </summary>
        /// <returns>true:リアクティブ可能, false:リアクティブ不可能。</returns>
        private bool CanReactive()
        {
            if (SheetState == null)
            {
                return false;
            }
            if (Sheet == null)
            {
                return false;
            }
            if (SheetState.IsCellValueChanged)
            {
                return false;
            }

            return true;
        }


    }
}
