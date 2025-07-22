using FarPoint.Win.Spread;
using System.Collections.Generic;

namespace Metroit.Win.GcSpread.MultiRow.Collections
{
    /// <summary>
    /// 1レコードを複数行として扱う インターフェースを提供します。
    /// </summary>
    public interface IMultiRowSheet
    {
        /// <summary>
        /// 扱っているシートを取得します。
        /// </summary>
        SheetView Sheet { get; }

        /// <summary>
        /// 1レコードに対する行数を取得します。
        /// </summary>
        int RowsPerRecord { get; }

        /// <summary>
        /// アイテムデータを取得します。
        /// </summary>
        //IBindingList Rows { get; }
        IEnumerable<object> Rows { get; }

        /// <summary>
        /// 新しいアイテムを生成します。
        /// </summary>
        /// <returns>新しいアイテム。</returns>
        object NewItem();

        /// <summary>
        /// 行を追加します。
        /// </summary>
        /// <returns>追加された行のアイテム。</returns>
        object AddRow();

        /// <summary>
        /// 行を追加します。
        /// </summary>
        /// <param name="row">追加するアイテム。</param>
        /// <returns>追加されたアイテム</returns>
        object AddRow(object row);

        /// <summary>
        /// 行を追加します。
        /// </summary>
        /// <param name="rows">追加するアイテム。</param>
        void AddRowRange(IEnumerable<object> rows);

        /// <summary>
        /// 画面の行インデックスから、行を削除します。
        /// </summary>
        /// <param name="index">画面の行インデックス。</param>
        void RemoveRow(int index);

        /// <summary>
        /// 行を削除します。
        /// </summary>
        /// <param name="row">削除するアイテム。</param>
        void RemoveRow(object row);

        /// <summary>
        /// アイテムのインデックスから、行を削除します。
        /// </summary>
        /// <param name="index">アイテムのインデックス。</param>
        void RemoveRowItemIndex(int index);

        /// <summary>
        /// すべての要素をクリアします。
        /// 削除されたことは通知されません。
        /// </summary>
        void Clear();

        /// <summary>
        /// 画面の行インデックスから、アイテムのインデックスを取得します。
        /// </summary>
        /// <param name="index">画面の行インデックス。</param>
        /// <returns>アイテムのインデックス。</returns>
        int GetItemIndex(int index);

        /// <summary>
        /// 画面の行インデックスから、アイテムの MultiRowAttribute で指定された行インデックスを取得します。
        /// </summary>
        /// <param name="index">画面の行インデックス。</param>
        /// <returns>アイテムの MultiRowAttribute で指定された行インデックス。</returns>
        int GetItemRowIndex(int index);

        /// <summary>
        /// 画面の行インデックスから、アイテムを取得します。
        /// </summary>
        /// <param name="index">画面の行インデックス。</param>
        /// <returns>アイテム。</returns>
        object GetItem(int index);

        /// <summary>
        /// オブジェクトを破棄します。
        /// </summary>
        void Dispose();
    }
}
