using FarPoint.Win.Spread;
using System.Collections.Generic;

namespace Metroit.Win.GcSpread.MultiRow
{
    /// <summary>
    /// 1レコード分の行のセットアップを行うためのデリゲートを提供します。
    /// </summary>
    /// <param name="viewStartRowIndex">1レコードが追加された画面の開始行インデックス。</param>
    /// <param name="viewRows">追加された <see cref="Row"/> オブジェクトのリスト。</param>
    public delegate void RowSetup(int viewStartRowIndex, IReadOnlyList<Row> viewRows);
}
