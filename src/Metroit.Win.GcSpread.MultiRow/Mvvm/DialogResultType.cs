namespace Metroit.Mvvm
{
    /// <summary>
    /// 指示結果を表します。
    /// </summary>
    public enum DialogResultType
    {
        /// <summary>
        /// /// 指示結果が設定されていません。
        /// </summary>
        None,

        /// <summary>
        /// /// 指示結果が OK です。
        /// </summary>
        OK,

        /// <summary>
        /// /// 指示結果がキャンセルされました。
        /// </summary>
        Cancel,

        /// <summary>
        /// /// 指示結果が中止されました。
        /// </summary>
        Abort,

        /// <summary>
        /// /// 指示結果がリトライされました。
        /// </summary>
        Retry,

        /// <summary>
        /// /// 指示結果が無視されました。
        /// </summary>
        Ignore,

        /// <summary>
        /// /// 指示結果が Yes です。
        /// </summary>
        Yes,

        /// <summary>
        /// /// 指示結果が No です。
        /// </summary>
        No,
    }
}
