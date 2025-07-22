namespace Metroit.Win.GcSpread.MultiRow.Metroit.Collections.Generic
{
    /// <summary>
    /// 追跡を行ったアイテムのインターフェースを提供します。
    /// </summary>
    public interface ITrackingItem<T>
    {
        /// <summary>
        /// 最後にアクセスされたアイテムを取得します。
        /// </summary>
        T LastAccessItem { get; }
    }
}
