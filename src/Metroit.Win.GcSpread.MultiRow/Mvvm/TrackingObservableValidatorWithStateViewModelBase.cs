using Metroit.Annotations;
using Metroit.Mvvm.ViewModels;

namespace Metroit.CommunityToolkit.Mvvm
{
    /// <summary>
    /// ViewModel の基底となる操作を提供します。
    /// </summary>
    /// <typeparam name="T">状態管理と変更追跡を行うクラス。</typeparam>
    public class TrackingObservableValidatorWithStateViewModelBase<T> : TrackingObservableValidatorWithState<T> where T : class
    {
        /// <summary>
        /// View制御サービスを提供します。
        /// </summary>
        [NoTracking]
        public ViewService ViewService { get; }

        /// <summary>
        /// 新しいインスタンスを生成します。
        /// </summary>
        public TrackingObservableValidatorWithStateViewModelBase() : base() { }

        /// <summary>
        /// 新しいインスタンスを生成します。
        /// </summary>
        /// <param name="viewService">View制御サービス。</param>
        public TrackingObservableValidatorWithStateViewModelBase(ViewService viewService) : base()
        {
            ViewService = viewService;
        }
    }
}
