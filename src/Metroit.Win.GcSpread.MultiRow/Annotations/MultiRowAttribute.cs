using System;

namespace Metroit.Win.GcSpread.MultiRow.Annotations
{
    /// <summary>
    /// 1レコード複数行のデータの定義を提供します。
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class MultiRowAttribute : Attribute
    {
        /// <summary>
        /// 行インデックスを取得します。
        /// </summary>
        public int Row { get; }

        /// <summary>
        /// 列インデックスを取得します。
        /// </summary>
        public int Column { get; }

        /// <summary>
        /// 新しいインスタンスを生成します。
        /// </summary>
        /// <param name="row">行インデックス。</param>
        /// <param name="column">列インデックス。</param>
        public MultiRowAttribute(int row, int column)
        {
            Row = row;
            Column = column;
        }
    }
}
