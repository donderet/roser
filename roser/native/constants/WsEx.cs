﻿namespace roser.native
{
    public enum WsEx : uint
    {
		DlgModalFrame = 0x00000001,
		NoParentNotify = 0x00000004,
		TopMost = 0x00000008,
		AcceptFiles = 0x00000010,
		Transparent = 0x00000020,
		MdiChild = 0x00000040,
		ToolWindow = 0x00000080,
		WindowEdge = 0x00000100,
		ClientEdge = 0x00000200,
		ContextHelp = 0x00000400,
		Right = 0x00001000,
		Left = 0x00000000,
		RtlReading = 0x00002000,
		LtrReading = 0x00000000,
		LeftScrollbar = 0x00004000,
		RightScrollbar = 0x00000000,
		ControlParent = 0x00010000,
		StaticEdge = 0x00020000,
		AppWindow = 0x00040000,

		OverlappedWindow = WindowEdge | ClientEdge,
		PaletteWindow = WindowEdge | ToolWindow | TopMost,
		Layered = 0x00080000,
		NoInheritLayout = 0x00100000,
		NoRedirectionBitmap = 0x00200000,
		LayoutRtl = 0x00400000,
		Composited = 0x02000000,
		NoActivate = 0x08000000
	}
}
