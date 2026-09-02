namespace Metroit.Win.GcSpread.MultiRow
{
    /// <summary>
    /// 行が削除されたときのイベントデータを提供します。
    /// </summary>
    public class RowRemovedEventArgs
    {
        /// <summary>
        /// 削除された画面の開始行インデックスを取得します。<br/>
        /// この行インデックスに該当する画面の行はすでに存在しません。
        /// </summary>
        public int RemovedViewStartRowIndex { get; }

        /// <summary>
        /// 削除された画面の終了行インデックスを取得します。<br/>
        /// この行インデックスに該当する画面の行はすでに存在しません。
        /// </summary>
        public int RemovedViewEndRowIndex { get; }

        /// <summary>
        /// 新しいインスタンスを生成します。
        /// </summary>
        /// <param name="removedViewStartRowIndex">削除された画面の開始行インデックス</param>
        /// <param name="removedViewEndRowIndex">削除された画面の終了行インデックス</param>
        public RowRemovedEventArgs(int removedViewStartRowIndex, int removedViewEndRowIndex)
        {
            RemovedViewStartRowIndex = removedViewStartRowIndex;
            RemovedViewEndRowIndex = removedViewEndRowIndex;
        }
    }
}
