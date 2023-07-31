using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace SightKeeper.Avalonia.Views.Annotating;

public partial class ScreenshoterSettings : UserControl
{
    public ScreenshoterSettings()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}