namespace ExtractorSharp.Composition.Compatibility {

    /// <summary>
    /// 兼容WinForm和WPF的消息提示框
    /// </summary>
    public interface ICommonMessageBox {
        CommonMessageBoxResult Show(string title, string message, CommonMessageBoxButton button = CommonMessageBoxButton.OK, CommonMessageBoxIcon icon = CommonMessageBoxIcon.None);
    }

    public enum CommonMessageBoxButton {
        OK = 0,
        OKCancel = 1,
        AbortRetryIgnore = 2,
        YesNoCancel = 3,
        YesNo = 4,
        RetryCancel = 5
    }


    public enum CommonMessageBoxIcon {
        None = 0,
        Hand = 16,
        Stop = 16,
        Error = 16,
        Question = 32,
        Exclamation = 48,
        Warning = 48,
        Asterisk = 64,
        Information = 64
    }

    public enum CommonMessageBoxResult {
        None = 0,
        OK = 1,
        Cancel = 2,
        Yes = 6,
        No = 7
    }
}
