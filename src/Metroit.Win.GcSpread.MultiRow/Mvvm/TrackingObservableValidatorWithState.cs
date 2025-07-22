using Metroit.Annotations;

namespace Metroit.CommunityToolkit.Mvvm
{
    /// <summary>
    /// 状態を持つ変更追跡が可能なオブジェクトを提供します。
    /// </summary>
    /// <typeparam name="T">状態管理と変更追跡を行うクラス。</typeparam>
    public class TrackingObservableValidatorWithState<T> : TrackingObservableValidator<T>, IStateObject where T : class
    {
        private ItemState _state = ItemState.New;

        /// <summary>
        /// 現在の状態を取得します。
        /// </summary>
        [NoTracking]
        public ItemState State => _state;

        /// <summary>
        /// 新しいインスタンスを生成します。
        /// </summary>
        public TrackingObservableValidatorWithState() : base() { }

        /// <summary>
        /// 状態を変更します。
        /// </summary>
        /// <param name="state">状態。</param>
        public void ChangeState(ItemState state)
        {
            _state = state;
        }
    }
}
