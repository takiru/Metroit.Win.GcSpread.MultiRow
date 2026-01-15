using FarPoint.Win.Spread;
using System.Collections.Generic;

namespace Metroit.Win.GcSpread.MultiRow
{
    /// <summary>
    /// 1レコード分の行のセットアップを行うためのデリゲートを提供します。
    /// </summary>
    /// <param name="rowIndex">1レコードが追加された実際の開始行インデックス。</param>
    /// <param name="rows">追加された行オブジェクトのコレクション。</param>
    public delegate void RowSetupDelegate(int rowIndex, IEnumerable<Row> rows);
}
