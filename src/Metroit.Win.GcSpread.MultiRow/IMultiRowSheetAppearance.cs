using FarPoint.Win.Spread;
using System.Collections.Generic;
using System.Drawing;

namespace Metroit.Win.GcSpread.MultiRow
{
    /// <summary>
    /// 1レコードを複数行として扱うためのシートの外観を制御するインターフェースを提供します。
    /// </summary>
    internal interface IMultiRowSheetAppearance
    {
        /// <summary>
        /// 扱っているシートを取得します。
        /// </summary>
        SheetView Sheet { get; }

        /// <summary>
        /// 交互に行の背景色を設定します。
        /// </summary>
        /// <param name="rowsPerRecord">1レコードに対する行数。</param>
        /// <param name="oddBackColor">奇数レコードの背景色。</param>
        /// <param name="evenBackColor">偶数レコードの背景色。</param>
        void SetAlternatingRowsBackColor(int rowsPerRecord, Color oddBackColor, Color evenBackColor);

        /// <summary>
        /// 行のリサイズを無効化します。
        /// </summary>
        /// <param name="rows">リサイズを無効化する行のコレクション。</param>
        void DisableRowResize(IEnumerable<Row> rows);
    }
}
