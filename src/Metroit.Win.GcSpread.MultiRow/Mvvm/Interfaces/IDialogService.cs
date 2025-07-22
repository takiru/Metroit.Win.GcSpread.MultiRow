namespace Metroit.Mvvm.Interfaces
{
    /// <summary>
    /// ダイアログのサービスを提供します。
    /// </summary>
    public interface IDialogService
    {
        /// <summary>
        /// ダイアログをモーダレスで表示します。
        /// </summary>
        /// <param name="windowKey">識別キー。</param>
        void Show(string windowKey);

        /// <summary>
        /// ダイアログをモーダレスで表示します。
        /// </summary>
        /// <param name="windowKey">識別キー。</param>
        /// <param name="request">リクエスト情報。</param>
        void Show(string windowKey, IDialogRequest request);

        /// <summary>
        /// ダイアログをモーダルで表示します。
        /// </summary>
        /// <param name="windowKey">識別キー。</param>
        /// <returns>レスポンス情報。</returns>
        IDialogResponse ShowDialog(string windowKey);

        /// <summary>
        /// ダイアログをモーダルで表示します。
        /// </summary>
        /// <param name="windowKey">識別キー。</param>
        /// <param name="request">リクエスト情報。</param>
        /// <returns>レスポンス情報。</returns>
        IDialogResponse ShowDialog(string windowKey, IDialogRequest request);
    }
}
