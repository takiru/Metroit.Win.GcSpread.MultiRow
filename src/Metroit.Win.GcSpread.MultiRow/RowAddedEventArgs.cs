using FarPoint.Win.Spread;
using System.Collections.Generic;

namespace Metroit.Win.GcSpread.MultiRow
{
    /// <summary>
    /// 行が追加されたときのイベントデータを提供します。
    /// </summary>
    public class RowAddedEventArgs
    {
        /// <summary>
        /// 追加された画面の開始行インデックスを取得します。
        /// </summary>
        public int AddedViewStartRowIndex { get; }

        /// <summary>
        /// 追加された画面の行リストを取得します。
        /// </summary>
        public IReadOnlyList<Row> ViewRows { get; }

        /// <summary>
        /// 新しいインスタンスを生成します。
        /// </summary>
        /// <param name="addedViewStartRowIndex">追加された画面の開始行インデックス。</param>
        /// <param name="viewRows">追加された画面の行リスト。</param>
        public RowAddedEventArgs(int addedViewStartRowIndex, IReadOnlyList<Row> viewRows)
        {
            AddedViewStartRowIndex = addedViewStartRowIndex;
            ViewRows = viewRows;
        }
    }
}
