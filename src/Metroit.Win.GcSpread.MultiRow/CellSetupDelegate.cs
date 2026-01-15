using FarPoint.Win.Spread;
using Metroit.Win.GcSpread.MultiRow.Annotations;

namespace Metroit.Win.GcSpread.MultiRow
{
    /// <summary>
    /// セルのセットアップを行うためのデリゲートを提供します。
    /// </summary>
    /// <param name="rowIndex"><see cref="MultiRowAttribute"/>で指定された行インデックス。</param>
    /// <param name="cell">セットアップ対象となる<see cref="Cell"/>。</param>
    public delegate void CellSetupDelegate(int rowIndex, Cell cell);
}
