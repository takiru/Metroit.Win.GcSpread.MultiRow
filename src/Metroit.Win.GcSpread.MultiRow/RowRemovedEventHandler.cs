namespace Metroit.Win.GcSpread.MultiRow
{
    /// <summary>
    /// 行が削除されたときに発生します。
    /// </summary>
    /// <param name="sender">イベントの発生元オブジェクト。</param>
    /// <param name="e">行が削除されたときのイベントデータ。</param>
    public delegate void RowRemovedEventHandler(object sender, RowRemovedEventArgs e);
}
