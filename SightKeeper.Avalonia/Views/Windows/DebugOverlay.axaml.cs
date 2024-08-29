using Avalonia.Controls;
using CommunityToolkit.Diagnostics;
#if OS_WINDOWS
using SightKeeper.Application.Windows;
#endif

namespace SightKeeper.Avalonia.Views.Windows;

internal partial class DebugOverlay : Window
{
    public DebugOverlay()
    {
        InitializeComponent();
        var platformHandle = TryGetPlatformHandle();
        Guard.IsNotNull(platformHandle);
#if OS_WINDOWS
        User32.MakeWindowClickThrough(platformHandle.Handle);
#endif
    }

    public string Text
    {
        get => TextBlock.Text ?? string.Empty;
        set => TextBlock.Text = value;
    }
}