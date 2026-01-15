using FarPoint.Win.Spread;
using GrapeCity.Spreadsheet;
using System.Collections.Generic;

namespace Metroit.Win.GcSpread.MultiRow
{
    /// <summary>
    /// <see cref="FpSpread.LegacyBehaviors"/>に<see cref="LegacyBehaviors.Style"/>が設定されていないときの外観を提供します。
    /// </summary>
    internal class UnstyledRowAppearance : IMultiRowSheetAppearance
    {
        /// <summary>
        /// 扱っているシートを取得します。
        /// </summary>
        public SheetView Sheet { get; }

        /// <summary>
        /// 新しいインスタンスを生成します。
        /// </summary>
        /// <param name="sheet">扱うシート。</param>
        public UnstyledRowAppearance(SheetView sheet)
        {
            Sheet = sheet;
        }

        /// <summary>
        /// 交互に行の背景色を設定します。
        /// </summary>
        /// <param name="rowsPerRecord">1レコードに対する行数。</param>
        /// <param name="oddBackColor">奇数レコードの背景色。</param>
        /// <param name="evenBackColor">偶数レコードの背景色。</param>
        public void SetAlternatingRowsBackColor(int rowsPerRecord, System.Drawing.Color oddBackColor, System.Drawing.Color evenBackColor)
        {
            IWorksheet worksheet = Sheet.AsWorksheet();
            worksheet.Styles.AlternatingRows.Count = rowsPerRecord * 2;

            // 奇数レコード
            for (var i = 0; i < rowsPerRecord; i++)
            {
                worksheet.Styles.AlternatingRows[i].Interior.Color = Color.FromArgb(oddBackColor.ToArgb());
            }

            // 偶数レコード
            for (var i = rowsPerRecord; i < worksheet.Styles.AlternatingRows.Count; i++)
            {
                worksheet.Styles.AlternatingRows[i].Interior.Color = Color.FromArgb(evenBackColor.ToArgb());
            }
        }

        /// <summary>
        /// 行のリサイズを無効化します。
        /// </summary>
        /// <param name="rows">リサイズを無効化する行のコレクション。</param>
        public void DisableRowResize(IEnumerable<Row> rows)
        {
            foreach (var row in rows)
            {
                row.Resizable = false;
            }
        }
    }
}
