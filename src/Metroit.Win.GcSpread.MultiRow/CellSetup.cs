using FarPoint.Win.Spread;
using Metroit.Win.GcSpread.MultiRow.Annotations;

namespace Metroit.Win.GcSpread.MultiRow
{
    /// <summary>
    /// セルのセットアップを行うためのデリゲートを提供します。
    /// </summary>
    /// <param name="rowIndexPerRecord"><see cref="MultiRowAttribute"/> で指定されている行インデックス。</param>
    /// <param name="cell">セットアップ対象となる <see cref="Cell"/> オブジェクト。</param>
    public delegate void CellSetup(int rowIndexPerRecord, Cell cell);
}
