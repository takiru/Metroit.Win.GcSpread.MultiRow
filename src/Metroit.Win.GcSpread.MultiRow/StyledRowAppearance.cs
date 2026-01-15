using FarPoint.Win.Spread;
using System.Collections.Generic;
using System.Drawing;

namespace Metroit.Win.GcSpread.MultiRow
{
    /// <summary>
    /// <see cref="FpSpread.LegacyBehaviors"/>に<see cref="LegacyBehaviors.Style"/>が設定されているときの外観を提供します。
    /// </summary>
    internal class StyledRowAppearance : IMultiRowSheetAppearance
    {
        /// <summary>
        /// 扱っているシートを取得します。
        /// </summary>
        public SheetView Sheet { get; }

        /// <summary>
        /// 新しいインスタンスを生成します。
        /// </summary>
        /// <param name="sheet">扱うシート。</param>
        public StyledRowAppearance(SheetView sheet)
        {
            Sheet = sheet;
        }

        /// <summary>
        /// 交互に行の背景色を設定します。
        /// </summary>
        /// <param name="rowsPerRecord">1レコードに対する行数。</param>
        /// <param name="oddBackColor">奇数レコードの背景色。</param>
        /// <param name="evenBackColor">偶数レコードの背景色。</param>
        public void SetAlternatingRowsBackColor(int rowsPerRecord, Color oddBackColor, Color evenBackColor)
        {
            Sheet.AlternatingRows.Count = rowsPerRecord * 2;

            // 奇数レコード
            for (var i = 0; i < rowsPerRecord; i++)
            {
                Sheet.AlternatingRows[i].BackColor = oddBackColor;
            }

            // 偶数レコード
            for (var i = rowsPerRecord; i < Sheet.AlternatingRows.Count; i++)
            {
                Sheet.AlternatingRows[i].BackColor = evenBackColor;
            }
        }

        /// <summary>
        /// 行のリサイズを無効化します。
        /// </summary>
        /// <param name="rows">リサイズを無効化する行のコレクション。</param>
        public void DisableRowResize(IEnumerable<Row> rows)
        {
            Sheet.Rows.Default.Resizable = false;
        }
    }
}
