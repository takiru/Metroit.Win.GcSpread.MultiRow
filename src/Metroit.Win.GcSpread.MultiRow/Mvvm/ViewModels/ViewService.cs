using Metroit.Mvvm.Interfaces;

namespace Metroit.Mvvm.ViewModels
{
    /// <summary>
    /// View に関する制御用のサービスを提供します。
    /// </summary>
    public class ViewService
    {
        /// <summary>
        /// ダイアログサービスを提供します。
        /// </summary>
        public IDialogService Dialog { get; }

        /// <summary>
        /// メッセージサービスを提供します。
        /// </summary>
        public IMessageService<DialogResultType> Message { get; }

        /// <summary>
        /// 新しいインスタンスを生成します。
        /// </summary>
        /// <param name="dialog">ダイアログサービス。</param>
        /// <param name="message">メッセージサービス。</param>
        public ViewService(IDialogService dialog, IMessageService<DialogResultType> message)
        {
            Dialog = dialog;
            Message = message;
        }
    }
}
