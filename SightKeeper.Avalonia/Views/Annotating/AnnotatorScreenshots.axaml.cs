using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace SightKeeper.Avalonia.Views.Annotating;

public partial class AnnotatorScreenshots : UserControl
{
    public AnnotatorScreenshots()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}