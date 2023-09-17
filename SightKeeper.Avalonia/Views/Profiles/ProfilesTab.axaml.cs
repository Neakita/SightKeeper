using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace SightKeeper.Avalonia.Views.Tabs;

public partial class ProfilesTab : UserControl
{
    public ProfilesTab()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}