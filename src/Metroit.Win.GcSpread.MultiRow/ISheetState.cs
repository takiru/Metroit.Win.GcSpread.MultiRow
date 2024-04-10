namespace Metroit.Win.GcSpread.MultiRow
{
    /// <summary>
    /// シートの操作状態 のインターフェースを提供します。
    /// </summary>
    internal interface ISheetState
    {
        /// <summary>
        /// Cell の値が変更されたかどうかを取得または設定します。
        /// </summary>
        bool IsCellValueChanged { get; }
    }
}
