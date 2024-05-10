namespace roser.native
{
    [Flags]
    internal enum DwmWindowAttribute : uint
    {
        NcRenderingEnabled = 1,
        NCRENDERING_POLICY,
        TransitionsForceDisabled,
        AllowNcPaint,
        CaptionButtonBounds,
        NonClientRtlLayout,
        ForceIconicRepresentation,
        Flip3dPolicy,
        ExtendedFrameBounds,
        HasIconicBitmap,
        DisallowPeek,
        ExcludedFromPeek,
        Cloak,
        Cloaked,
        FreezeRepresentation,
		PassiveUpdateMode,
		UseHostBackdropBrush,
		UseImmersiveDarkMode = 20,
		WindowCornerPreference = 33,
		BorderColor,
		CaptionColor,
		TextColor,
		VisibleFrameBorderThickness,
		SystemBackdropType,
		Last,
	}
}
