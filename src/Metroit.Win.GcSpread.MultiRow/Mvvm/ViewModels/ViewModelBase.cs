namespace Metroit.Mvvm.ViewModels
{
    /// <summary>
    /// ViewModel の基底となる操作を提供します。
    /// </summary>
    public class ViewModelBase
    {
        /// <summary>
        /// View制御サービスを提供します。
        /// </summary>
        public ViewService ViewService { get; }

        /// <summary>
        /// 新しいインスタンスを生成します。
        /// </summary>
        public ViewModelBase() { }

        /// <summary>
        /// 新しいインスタンスを生成します。
        /// </summary>
        /// <param name="viewService">View制御サービス。</param>
        public ViewModelBase(ViewService viewService)
        {
            ViewService = viewService;
        }
    }
}
