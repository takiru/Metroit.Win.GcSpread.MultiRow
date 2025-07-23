namespace Metroit.Win.GcSpread.MultiRow
{
    /// <summary>
    /// アクション開始操作の種類を表します。
    /// </summary>
    public enum ActionBeginOperation
    {
        /// <summary>
        /// アクションなし。
        /// </summary>
        None,

        /// <summary>
        /// リストのアイテムによる操作。
        /// </summary>
        Item,

        /// <summary>
        /// 実際のセルによる操作。
        /// </summary>
        ActualCell
    }
}
