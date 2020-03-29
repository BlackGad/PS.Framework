using System;

namespace PS.Windows.Interop
{
    [Flags]
    public enum MessageBoxStyle : uint
    {
        /// <summary>
        ///     The message box contains three push buttons: Abort, Retry, and Ignore
        /// </summary>
        AbortRetryIgnore = 2,

        /// <summary>
        ///     Microsoft Windows 2000/XP: The message box contains three push buttons:
        ///     Cancel, Try Again, Continue. Use this message box type instead of ABORTRETRYIGNORE.
        /// </summary>
        CancelTryContinue = 6,

        /// <summary>
        ///     Windows 95/98/Me, Windows NT 4.0 and later: Adds a Help button to the message box.
        ///     When the user clicks the Help button or presses F1, the system sends a WM_HELP message
        ///     to the owner.
        /// </summary>
        Help = 16384,

        /// <summary>
        ///     The message box contains one push button: OK. This is the default.
        /// </summary>
        OK = 0,

        /// <summary>
        ///     The message box contains two push buttons: OK and Cancel.
        /// </summary>
        OkCancel = 1,

        /// <summary>
        ///     The message box contains two push buttons: Retry and Cancel.
        /// </summary>
        RetryCancel = 5,

        /// <summary>
        ///     The message box contains two push buttons: Yes and No.
        /// </summary>
        YesNo = 4,

        /// <summary>
        ///     The message box contains three push buttons: Yes, No, and Cancel.
        ///     To display an icon in the message box, specify one of the following values.
        /// </summary>
        YesNoCancel = 3,

        /// <summary>
        ///     An exclamation-point icon appears in the message box.
        /// </summary>
        IconExclamation = 48,

        /// <summary>
        ///     An exclamation-point icon appears in the message box.
        /// </summary>
        IconWarning = IconInformation,

        /// <summary>
        ///     An icon consisting of a lowercase letter i in a circle appears in the message box.
        /// </summary>
        IconInformation = 64,

        /// <summary>
        ///     An icon consisting of a lowercase letter i in a circle appears in the message box.
        /// </summary>
        IconAsterisk = IconInformation,

        /// <summary>
        ///     A question-mark icon appears in the message box. The question-mark message icon is
        ///     no longer recommended because it does not clearly represent a specific type of message
        ///     and because the phrasing of a message as a question could apply to any message type.
        ///     In addition, users can confuse the message symbol question mark with Help information.
        ///     Therefore, do not use this question mark message symbol in your message boxes. The system
        ///     continues to support its inclusion only for backward compatibility.
        /// </summary>
        IconQuestion = 32,

        /// <summary>
        ///     A stop-sign icon appears in the message box.
        /// </summary>
        IconStop = 16,

        /// <summary>
        ///     A stop-sign icon appears in the message box.
        /// </summary>
        IconError = IconStop,

        /// <summary>
        ///     A stop-sign icon appears in the message box.
        ///     To indicate the default button, specify one of the following values.
        /// </summary>
        IconHand = IconStop,

        /// <summary>
        ///     The first button is the default button.
        ///     DEFBUTTON1 is the default unless DEFBUTTON2, DEFBUTTON3, or DEFBUTTON4 is specified.
        /// </summary>
        DefButton1 = 0,

        /// <summary>
        ///     The second button is the default button.
        /// </summary>
        DefButton2 = 256,

        /// <summary>
        ///     The third button is the default button.
        /// </summary>
        DefButton3 = 512,

        /// <summary>
        ///     The fourth button is the default button.
        ///     To indicate the modality of the dialog box, specify one of the following values.
        /// </summary>
        DefButton4 = 768,

        /// <summary>
        ///     The user must respond to the message box before continuing work in the window identified by
        ///     the hWnd parameter. However, the user can move to the windows of other threads and work in
        ///     those windows.
        ///     Depending on the hierarchy of windows in the application, the user may be able to move to
        ///     other windows within the thread. All child windows of the parent of the message box are
        ///     automatically disabled, but pop-up windows are not.
        ///     APPLMODAL is the default if neither SYSTEMMODAL nor TASKMODAL is specified.
        /// </summary>
        ApplModal = 0,

        /// <summary>
        ///     Same as APPLMODAL except that the message box has the WS_EX_TOPMOST style.
        ///     Use system-modal message boxes to notify the user of serious, potentially damaging errors
        ///     that require immediate attention (for example, running out of memory). This flag has no
        ///     effect on the user's ability to interact with windows other than those associated with hWnd parameter
        ///     of MessageBox function.
        /// </summary>
        SystemModal = 4096,

        /// <summary>
        ///     Same as APPLMODAL except that all the top-level windows belonging to the current thread
        ///     are disabled if the hWnd parameter is null. Use this flag when the calling application or
        ///     library does not have a window handle available but still needs to prevent input to other
        ///     windows in the calling thread without suspending other threads.
        ///     To specify other options, use one or more of the following values.
        /// </summary>
        TaskModal = 8192,

        /// <summary>
        ///     Windows NT/2000/XP: Same as desktop of the interactive window station. For more information, see Window Stations.
        ///     Windows NT 4.0 and earlier: If the current input desktop is not the default desktop, MessageBox fails.
        ///     Windows 2000/XP: If the current input desktop is not the default desktop, MessageBox does not return until the user
        ///     switches to the default desktop.
        ///     Windows 95/98/Me: This flag has no effect.
        /// </summary>
        DefaultDesktopOnly = 131072,

        /// <summary>
        ///     The text is right-justified.
        /// </summary>
        Right = 524288,

        /// <summary>
        ///     Displays message and caption text using right-to-left reading order on Hebrew and Arabic systems.
        /// </summary>
        RtlReading = 1048576,

        /// <summary>
        ///     The message box becomes the foreground window. Internally, the system calls the SetForegroundWindow
        ///     function for the message box
        /// </summary>
        SetForeground = 65536,

        /// <summary>
        ///     The message box is created with the WS_EX_TOPMOST window style.
        /// </summary>
        Topmost = 262144,

        /// <summary>
        ///     Windows NT/2000/XP: The caller is a service notifying the user of an event. The function displays a
        ///     message box on the current active desktop, even if there is no user logged on to the computer.
        ///     Terminal Services: If the calling thread has an impersonation token, the function directs the
        ///     message box to the session specified in the impersonation token. If this flag is set, the hWnd
        ///     parameter of MessageBox function must be null. This is so that the message box can appear on a
        ///     desktop other than the desktop corresponding to the hWnd.
        /// </summary>
        ServiceNotification = 2097152,

        /// <summary>
        ///     Windows NT/2000/XP: This value corresponds to the value defined for SERVICE_NOTIFICATION for Windows NT version
        ///     3.51
        /// </summary>
        ServiceNotificationNt3X = 262144,

        /// <summary>
        ///     Specific for MSGBOXPARAMS structure to use in MessageBoxIndirect function. Indicates the message
        ///     box to display the icon specified by the lpszIcon member of MSGBOXPARAMS structure.
        /// </summary>
        UserIcon = 128
    }
}