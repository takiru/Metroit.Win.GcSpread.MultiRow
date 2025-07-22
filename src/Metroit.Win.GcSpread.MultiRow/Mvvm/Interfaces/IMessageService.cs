using System;

namespace Metroit.Mvvm.Interfaces
{
    /// <summary>
    /// メッセージのサービスを提供します。
    /// </summary>
    /// <typeparam name="T">指示結果の型。</typeparam>
    public interface IMessageService<T>
    {
        /// <summary>
        /// メッセージを表示するオーナーを取得します。
        /// </summary>
        Func<object> OwnerFormProvider { get; set; }

        /// <summary>
        /// 情報メッセージを表示します。
        /// </summary>
        /// <param name="message">メッセージ。</param>
        void Information(string message);

        /// <summary>
        /// 情報メッセージを表示します。
        /// </summary>
        /// <param name="message">メッセージ。</param>
        /// <param name="title">タイトル。</param>
        void Information(string message, string title);

        /// <summary>
        /// 確認メッセージを表示します。
        /// </summary>
        /// <param name="message">メッセージ。</param>
        /// <returns>指示結果。</returns>
        T ConfirmYesNo(string message);

        /// <summary>
        /// 確認メッセージを表示します。
        /// </summary>
        /// <param name="message">メッセージ。</param>
        /// <param name="defaultButton">既定のボタン。</param>
        /// <returns>指示結果。</returns>
        T ConfirmYesNo(string message, MessageBoxDefaultButtonType defaultButton);

        /// <summary>
        /// 確認メッセージを表示します。
        /// </summary>
        /// <param name="message">メッセージ。</param>
        /// <param name="title">タイトル。</param>
        /// <returns>指示結果。</returns>
        T ConfirmYesNo(string message, string title);

        /// <summary>
        /// 確認メッセージを表示します。
        /// </summary>
        /// <param name="message">メッセージ。</param>
        /// <param name="title">タイトル。</param>
        /// <param name="defaultButton">既定のボタン。</param>
        /// <returns>指示結果。</returns>
        T ConfirmYesNo(string message, string title, MessageBoxDefaultButtonType defaultButton);

        /// <summary>
        /// 確認メッセージを表示します。
        /// </summary>
        /// <param name="message">メッセージ。</param>
        /// <returns>指示結果。</returns>
        T ConfirmYesNoCancel(string message);

        /// <summary>
        /// 確認メッセージを表示します。
        /// </summary>
        /// <param name="message">メッセージ。</param>
        /// <param name="defaultButton">既定のボタン。</param>
        /// <returns>指示結果。</returns>
        T ConfirmYesNoCancel(string message, MessageBoxDefaultButtonType defaultButton);

        /// <summary>
        /// 確認メッセージを表示します。
        /// </summary>
        /// <param name="message">メッセージ。</param>
        /// <param name="title">タイトル。</param>
        /// <returns>指示結果。</returns>
        T ConfirmYesNoCancel(string message, string title);

        /// <summary>
        /// 確認メッセージを表示します。
        /// </summary>
        /// <param name="message">メッセージ。</param>
        /// <param name="title">タイトル。</param>
        /// <param name="defaultButton">既定のボタン。</param>
        /// <returns>指示結果。</returns>
        T ConfirmYesNoCancel(string message, string title, MessageBoxDefaultButtonType defaultButton);

        /// <summary>
        /// 確認メッセージを表示します。
        /// </summary>
        /// <param name="message">メッセージ。</param>
        /// <returns>指示結果。</returns>
        T ConfirmOkCancel(string message);

        /// <summary>
        /// 確認メッセージを表示します。
        /// </summary>
        /// <param name="message">メッセージ。</param>
        /// <param name="defaultButton">既定のボタン。</param>
        /// <returns>指示結果。</returns>
        T ConfirmOkCancel(string message, MessageBoxDefaultButtonType defaultButton);

        /// <summary>
        /// 確認メッセージを表示します。
        /// </summary>
        /// <param name="message">メッセージ。</param>
        /// <param name="title">タイトル。</param>
        /// <returns>指示結果。</returns>
        T ConfirmOkCancel(string message, string title);

        /// <summary>
        /// 確認メッセージを表示します。
        /// </summary>
        /// <param name="message">メッセージ。</param>
        /// <param name="title">タイトル。</param>
        /// <param name="defaultButton">既定のボタン。</param>
        /// <returns>指示結果。</returns>
        T ConfirmOkCancel(string message, string title, MessageBoxDefaultButtonType defaultButton);

        /// <summary>
        /// 警告メッセージを表示します。
        /// </summary>
        /// <param name="message">メッセージ。</param>
        void Warning(string message);

        /// <summary>
        /// 警告メッセージを表示します。
        /// </summary>
        /// <param name="message">メッセージ。</param>
        /// <param name="title">タイトル。</param>
        void Warning(string message, string title);

        /// <summary>
        /// エラーメッセージを表示します。
        /// </summary>
        /// <param name="message">メッセージ。</param>
        void Error(string message);

        /// <summary>
        /// エラーメッセージを表示します。
        /// </summary>
        /// <param name="message">メッセージ。</param>
        /// <param name="title">タイトル。</param>
        void Error(string message, string title);
    }
}
