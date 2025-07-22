using Metroit.Mvvm.ViewModels;

namespace Metroit.CommunityToolkit.Mvvm
{
    /// <summary>
    /// ViewModel の基底となる操作を提供します。
    /// </summary>
    public class TrackingObservableValidatorViewModelBase : TrackingObservableObject<TrackingObservableValidatorViewModelBase>
    {
        /// <summary>
        /// View制御サービスを提供します。
        /// </summary>
        public ViewService ViewService { get; }

        /// <summary>
        /// 新しいインスタンスを生成します。
        /// </summary>
        public TrackingObservableValidatorViewModelBase() { }

        /// <summary>
        /// 新しいインスタンスを生成します。
        /// </summary>
        /// <param name="viewService">View制御サービス。</param>
        public TrackingObservableValidatorViewModelBase(ViewService viewService)
        {
            ViewService = viewService;
        }
    }
}
