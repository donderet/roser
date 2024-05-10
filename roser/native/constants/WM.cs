﻿namespace roser.native
{
    public enum WM : uint
    {
        Null = 0x0000,
        Create = 0x0001,
        Destroy = 0x0002,
        Move = 0x0003,
        Size = 0x0005,
        Activate = 0x0006,
        SetFocus = 0x0007,
        KillFocus = 0x0008,
        Enable = 0x000A,
        SetRedraw = 0x000B,
        SetText = 0x000C,
        GetText = 0x000D,
        GetTextLength = 0x000E,
        Paint = 0x000F,
        Close = 0x0010,
        QueryEndSession = 0x0011,
        QueryOpen = 0x0013,
        EndSession = 0x0016,
        Quit = 0x0012,
        EraseBkgnd = 0x0014,
        SysColorChange = 0x0015,
        ShowWindow = 0x0018,
        WinIniChange = 0x001A,
        SettingChange = WinIniChange,
        DevModeChange = 0x001B,
        ActivateApp = 0x001C,
        FontChange = 0x001D,
        TimeChange = 0x001E,
        CancelMode = 0x001F,
        SetCursor = 0x0020,
        MouseActivate = 0x0021,
        ChildActivate = 0x0022,
        QueueSync = 0x0023,
        GetMinMaxInfo = 0x0024,
        PaintIcon = 0x0026,
        IconEraseBkgnd = 0x0027,
        NextDlgCtl = 0x0028,
        SpoolerStatus = 0x002A,
        DrawItem = 0x002B,
        MeasureItem = 0x002C,
        DeleteItem = 0x002D,
        VkeyToItem = 0x002E,
        CharToItem = 0x002F,
        SetFont = 0x0030,
        GetFont = 0x0031,
        SetHotkey = 0x0032,
        GetHotkey = 0x0033,
        QueryDragIcon = 0x0037,
        CompareItem = 0x0039,
        GetObject = 0x003D,
        Compacting = 0x0041,
        WindowPosChanging = 0x0046,
        WindowPosChanged = 0x0047,
        Power = 0x0048,
        CopyData = 0x004A,
        CancelJournal = 0x004B,
        InputlangChangerequest = 0x0050,
        InputlangChange = 0x0051,
        TCard = 0x0052,
        Help = 0x0053,
        UserChanged = 0x0054,
        ContextMenu = 0x007B,
        StyleChanging = 0x007C,
        StyleChanged = 0x007D,
        DisplayChange = 0x007E,
        GetIcon = 0x007F,
        SetIcon = 0x0080,
        NcCreate = 0x0081,
        NcDestroy = 0x0082,
        NcCalcSize = 0x0083,
        NcHitTest = 0x0084,
        NcPaint = 0x0085,
        NcActivate = 0x0086,
        GetDlgCode = 0x0087,
        SyncPaint = 0x0088,
        NcMouseMove = 0x00A0,
        NclButtonDown = 0x00A1,
        NclButtonup = 0x00A2,
        NclButtonDblClk = 0x00A3,
        NcrButtonDown = 0x00A4,
        NcrButtonup = 0x00A5,
        NcrButtonDblClk = 0x00A6,
        NcmButtonDown = 0x00A7,
        NcmButtonup = 0x00A8,
        NcmButtonDblClk = 0x00A9,
        NcxButtonDown = 0x00AB,
        NcxButtonup = 0x00AC,
        NcxButtonDblClk = 0x00AD,
        InputDeviceChange = 0x00fe,
        Input = 0x00FF,
        KeyFirst = 0x0100,
        KeyDown = 0x0100,
        KeyUp = 0x0101,
        Char = 0x0102,
        DeadChar = 0x0103,
        SysKeyDown = 0x0104,
        SysKeyup = 0x0105,
        SysChar = 0x0106,
        SysDeadChar = 0x0107,
        UniChar = 0x0109,
        KeyLast = 0x0109,
        // if _WIN32_WINNT < 0x0501 KEYLAST = 0x0108
        ImeStartComposition = 0x010D,
        ImeEndComposition = 0x010E,
        ImeComposition = 0x010F,
        ImeKeylast = 0x010F,
        InitDialog = 0x0110,
        Command = 0x0111,
        SysCommand = 0x0112,
        Timer = 0x0113,
        HScroll = 0x0114,
        VScroll = 0x0115,
        InitMenu = 0x0116,
        InitMenuPopup = 0x0117,
        MenuSelect = 0x011F,
        Gesture = 0x0119,
        MenuChar = 0x0120,
        EnterIdle = 0x0121,
        MenuRButtonUp = 0x0122,
        MenuDrag = 0x0123,
        MenuGetObject = 0x0124,
        UninitMenuPopup = 0x0125,
        MenuCommand = 0x0126,
        ChangeUiState = 0x0127,
        UpdateUiState = 0x0128,
        QueryUiState = 0x0129,
        CtlColorMsgBox = 0x0132,
        CtlColorEdit = 0x0133,
        CtlColorListBox = 0x0134,
        CtlColorBtn = 0x0135,
        CtlColorDlg = 0x0136,
        CtlColorScrollbar = 0x0137,
        CtlColorStatic = 0x0138,
        MouseFirst = 0x0200,
        MouseMove = 0x0200,
        LButtonDown = 0x0201,
        LButtonUp = 0x0202,
        LButtonDblClk = 0x0203,
        RButtonDown = 0x0204,
        RButtonUp = 0x0205,
        RButtonDblClk = 0x0206,
        MButtonDown = 0x0207,
        MButtonUp = 0x0208,
        MButtonDblClk = 0x0209,
        MouseWheel = 0x020A,
        XButtonDown = 0x020B,
        XButtonUp = 0x020C,
        XButtonDblClk = 0x020D,
        MousehWheel = 0x020e,
        MouseLast = 0x020e,
        EnterMenuLoop = 0x0211,
        ExitMenuLoop = 0x0212,
        NextMenu = 0x0213,
        Sizing = 0x0214,
        CaptureChanged = 0x0215,
        Moving = 0x0216,
        Powerbroadcast = 0x0218,
        DeviceChange = 0x0219,
        MdiCreate = 0x0220,
        MdiDestroy = 0x0221,
        MdiActivate = 0x0222,
        MdiRestore = 0x0223,
        MdiNext = 0x0224,
        MdiMaximize = 0x0225,
        MdiTile = 0x0226,
        MdiCascade = 0x0227,
        MdiIconArrange = 0x0228,
        MdiGetActive = 0x0229,
        MdiSetMenu = 0x0230,
        EnterSizeMove = 0x0231,
        ExitSizeMove = 0x0232,
        DropFiles = 0x0233,
        MdiRefreshMenu = 0x0234,
        PointerDeviceChange = 0x238,
        PointerDeviceInRange = 0x239,
        PointerDeviceOutOfRange = 0x23a,
        Touch = 0x0240,
        NcPointerUpdate = 0x0241,
        NcPointerDown = 0x0242,
        NcPointerup = 0x0243,
        PointerUpdate = 0x0245,
        PointerDown = 0x0246,
        PointerUp = 0x0247,
        PointerEnter = 0x0249,
        PointerLeave = 0x024a,
        PointerActivate = 0x024b,
        PointerCaptureChanged = 0x024c,
        TouchHitTesting = 0x024d,
        PointerWheel = 0x024e,
        PointerHWheel = 0x024f,
        PointerRoutedTo = 0x0251,
        PointerRoutedAway = 0x0252,
        PointerRoutedReleased = 0x0253,
        ImeSetContext = 0x0281,
        ImeNotify = 0x0282,
        ImeControl = 0x0283,
        ImeCompositionFull = 0x0284,
        ImeSelect = 0x0285,
        ImeChar = 0x0286,
        ImeRequest = 0x0288,
        ImeKeyDown = 0x0290,
        ImeKeyup = 0x0291,
        MouseHover = 0x02A1,
        MouseLeave = 0x02A3,
        NcMouseHover = 0x02A0,
        NcMouseLeave = 0x02A2,
        WtsSessionChange = 0x02B1,
        TabletFirst = 0x02c0,
        TabletLast = 0x02df,
        DpiChanged = 0x02e0,
        DpiChangedBeforeparent = 0x02e2,
        DpiChangedAfterparent = 0x02e3,
        GetDpiScaledSize = 0x02e4,
        Cut = 0x0300,
        Copy = 0x0301,
        Paste = 0x0302,
        Clear = 0x0303,
        Undo = 0x0304,
        RenderFormat = 0x0305,
        RenderAllFormats = 0x0306,
        DestroyClipboard = 0x0307,
        DrawClipboard = 0x0308,
        PaintClipboard = 0x0309,
        VScrollClipboard = 0x030A,
        SizeClipboard = 0x030B,
        AskCbFormatName = 0x030C,
        ChangeCbChain = 0x030D,
        HScrollClipboard = 0x030E,
        QueryNewPalette = 0x030F,
        PaletteIsChanging = 0x0310,
        PaletteChanged = 0x0311,
        Hotkey = 0x0312,
        Print = 0x0317,
        PrintClient = 0x0318,
        AppCommand = 0x0319,
        ThemeChanged = 0x031A,
        ClipboardUpdate = 0x031d,
        DwmCompositionChanged = 0x031e,
        DwmNcRenderingChanged = 0x031f,
        DwColorizationColorChanged = 0x0320,
        DwmWindowMaximizedChange = 0x0321,
        DwmSendIconicThumbnail = 0x0323,
        DwmSendIconicLivePreviewBitmap = 0x0326,
        GetTitleBarInfoEx = 0x033f,
        HandheldFirst = 0x0358,
        HandheldLast = 0x035F,
        AfxFirst = 0x0360,
        AfxLast = 0x037F,
        PenWinFirst = 0x0380,
        PenWinLast = 0x038F,
        App = 0x8000,
        User = 0x0400,
    }
}
