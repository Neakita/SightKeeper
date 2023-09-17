using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace SightKeeper.Avalonia.Views;

public partial class ProfileEditor : UserControl
{
    public ProfileEditor()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}