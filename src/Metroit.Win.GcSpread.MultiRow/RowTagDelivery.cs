using FarPoint.Win.Spread;
using System;

namespace Metroit.Win.GcSpread.MultiRow
{
    /// <summary>
    /// 行タグに含まれる<typeparamref name="T"/>の情報を設定・取得するための機能を提供します。
    /// </summary>
    /// <typeparam name="T">タグに含まれる取得・設定したい情報。</typeparam>
    public class RowTagDelivery<T>
    {
        /// <summary>
        /// タグの情報を設定する制御を取得します。
        /// </summary>
        public Action<Row, T> SetTag { get; }

        /// <summary>
        /// タグの情報を取得する制御を取得します。
        /// </summary>
        public Func<object, T> GetTag { get; }

        /// <summary>
        /// 新しいインスタンスを生成します。
        /// </summary>
        /// <param name="setTag">タグの情報を設定する制御。</param>
        /// <param name="getTag">タグの情報を取得する制御。</param>
        public RowTagDelivery(Action<Row, T> setTag, Func<object, T> getTag)
        {
            SetTag = setTag;
            GetTag = getTag;
        }
    }
}
