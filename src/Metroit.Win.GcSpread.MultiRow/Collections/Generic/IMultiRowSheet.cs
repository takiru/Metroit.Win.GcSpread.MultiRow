using Metroit.Collections.Generic;
using System.Collections.Generic;

namespace Metroit.Win.GcSpread.MultiRow.Collections.Generic
{
    /// <summary>
    /// 1レコードを複数行として扱うインターフェースを提供します。
    /// </summary>
    /// <typeparam name="T">StateKnownMultiRowItemBase を有する型。</typeparam>
    public interface IMultiRowSheet<T> : IMultiRowSheet where T : StateKnownMultiRowItemBase, new()
    {
        /// <summary>
        /// アイテムデータを取得します。
        /// </summary>
        new ItemRemovedKnownList<T> Rows { get; }
        //new IReadOnlyList<T> Rows { get; }

        /// <summary>
        /// 新しいアイテムを生成します。
        /// </summary>
        /// <returns>新しいアイテム。</returns>
        new T NewItem();

        /// <summary>
        /// 行を追加します。
        /// </summary>
        /// <returns>追加された行のアイテム。</returns>
        new T AddRow();

        /// <summary>
        /// 行を追加します。
        /// </summary>
        /// <param name="row">追加するアイテム。</param>
        /// <returns>追加されたアイテム</returns>
        T AddRow(T row);

        /// <summary>
        /// 行を追加します。
        /// </summary>
        /// <param name="rows">追加するアイテム。</param>
        void AddRowRange(IEnumerable<T> rows);

        /// <summary>
        /// 行を削除します。
        /// </summary>
        /// <param name="row">削除するアイテム。</param>
        void RemoveRow(T row);

        /// <summary>
        /// 画面の行インデックスから、アイテムを取得します。
        /// </summary>
        /// <param name="index">画面の行インデックス。</param>
        /// <returns>アイテム。</returns>
        new T GetItem(int index);
    }
}
