using FarPoint.Win.Spread;
using Metroit.ChangeTracking;
using System;
using System.Drawing;

namespace Metroit.Win.GcSpread.MultiRow
{
    /// <summary>
    /// 1レコードを複数行として扱うための構成情報を提供します。
    /// </summary>
    /// <typeparam name="T">状態を持つ変更追跡が可能なクラス。</typeparam>
    public class MultiRowSheetConfiguration<T> where T : IPropertyChangeTrackerProvider, IStateObject, new()
    {
        /// <summary>
        /// 扱っているシートを取得します。
        /// </summary>
        public SheetView Sheet { get; protected set; }

        /// <summary>
        /// 1レコードに対する行数を取得します。
        /// </summary>
        public int RowsPerRecord { get; protected set; }

        /// <summary>
        /// 奇数レコードの背景色を取得または設定します。
        /// </summary>
        public Color OddBackColor { get; set; } = Color.Empty;

        /// <summary>
        /// 偶数レコードの背景色を取得または設定します。
        /// </summary>
        public Color EvenBackColor { get; set; } = Color.Empty;

        /// <summary>
        /// 1レコード分の行の追加が行われた時、1レコード分の行のセットアップ制御を取得または設定します。
        /// </summary>
        public RowSetupDelegate RowSetup { get; set; } = null;

        /// <summary>
        /// 1レコード分の行の行の追加が行われた時、セルのセットアップ制御を取得または設定します。
        /// </summary>
        public CellSetupDelegate CellSetup { get; set; } = null;

        /// <summary>
        /// タグの伝達制御を取得または設定します。
        /// </summary>
        public RowTagDelivery<T> TagDelivery { get; set; } = null;

        /// <summary>
        /// 新しいインスタンスを生成します。
        /// </summary>
        /// <param name="sheet">シートオブジェクト。</param>
        /// <param name="rowsPerRecord">1レコードの行数。</param>
        /// <exception cref="ArgumentNullException"><paramref name="sheet"/>が<see langword="null"/>です。</exception>
        /// <exception cref="ArgumentException"><paramref name="rowsPerRecord"/>が2未満です。</exception>
        public MultiRowSheetConfiguration(SheetView sheet, int rowsPerRecord)
        {
            if (sheet == null)
            {
                throw new ArgumentNullException(nameof(sheet));
            }
            if (rowsPerRecord < 2)
            {
                throw new ArgumentException("rowsPerRecord is less than 2.");
            }

            Sheet = sheet;
            RowsPerRecord = rowsPerRecord;
        }
    }
}
