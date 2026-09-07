using FarPoint.Win.Spread;
using Metroit.ChangeTracking;
using Metroit.Win.GcSpread.MultiRow.Annotations;
using System;
using System.Drawing;

namespace Metroit.Win.GcSpread.MultiRow
{
    /// <summary>
    /// 1レコードを複数行として扱うための構成情報を読み取り専用として提供します。
    /// </summary>
    /// <typeparam name="TRecord">状態を持つ変更追跡が可能なクラス。</typeparam>
    public class ReadOnlyMultiRowSheetConfiguration<TRecord> where TRecord : IPropertyChangeTrackerProvider, IStateObject
    {
        /// <summary>
        /// 扱っているシートを取得します。
        /// </summary>
        public SheetView Sheet => Configuration.Sheet;

        /// <summary>
        /// 1レコードに対する行数を取得します。
        /// </summary>
        public int RowsPerRecord => Configuration.RowsPerRecord;

        /// <summary>
        /// 奇数レコードの背景色を取得します。
        /// </summary>
        public Color OddBackColor => Configuration.OddBackColor;

        /// <summary>
        /// 偶数レコードの背景色を取得します。
        /// </summary>
        public Color EvenBackColor => Configuration.EvenBackColor;

        /// <summary>
        /// 1レコード分の行の追加が行われた時、1レコード分の行のセットアップ制御を取得します。
        /// </summary>
        public RowSetup RowSetup => Configuration.RowSetup;

        /// <summary>
        /// 1レコード分の行の行の追加が行われた時、セルのセットアップ制御を取得します。
        /// </summary>
        public CellSetup CellSetup => Configuration.CellSetup;

        /// <summary>
        /// タグの伝達制御を取得します。
        /// </summary>
        public RowTagDelivery<TRecord> TagDelivery => Configuration.TagDelivery;

        private MultiRowSheetConfiguration<TRecord> Configuration { get; }

        /// <summary>
        /// 新しいインスタンスを生成します。
        /// </summary>
        /// <param name="configuration">1レコードを複数行として扱うための構成情報。</param>
        /// <exception cref="ArgumentNullException"><paramref name="configuration"/>が<see langword="null"/>です。</exception>
        public ReadOnlyMultiRowSheetConfiguration(MultiRowSheetConfiguration<TRecord> configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            Configuration = configuration;
        }

        /// <summary>
        /// 画面の行インデックスから行番号を取得します。
        /// </summary>
        /// <param name="viewRowIndex">画面の行インデックス。</param>
        /// <returns>行番号。</returns>
        public int GetRowNumber(int viewRowIndex)
        {
            return viewRowIndex / RowsPerRecord + 1;
        }

        /// <summary>
        /// 画面の行インデックスから、<see cref="MultiRowAttribute"/> で指定されている行インデックスを取得します。
        /// </summary>
        /// <param name="viewRowIndex">画面の行インデックス。</param>
        /// <returns><see cref="MultiRowAttribute"/> で指定されている行インデックス。</returns>
        public int GetAttributeRowIndex(int viewRowIndex)
        {
            return viewRowIndex % RowsPerRecord;
        }

        /// <summary>
        /// 画面の開始行インデックスから、画面の終了行インデックスを取得します。
        /// </summary>
        /// <param name="viewStartRowIndex">画面の開始行インデックス。</param>
        /// <returns></returns>
        public int GetViewEndRowIndex(int viewStartRowIndex)
        {
            return viewStartRowIndex + RowsPerRecord - 1;
        }
    }
}
