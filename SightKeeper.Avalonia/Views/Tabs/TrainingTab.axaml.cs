using Avalonia.Controls;

namespace SightKeeper.Avalonia.Views.Tabs;

internal sealed partial class TrainingTab : UserControl
{
    public TrainingTab()
    {
        InitializeComponent();
    }

    protected override void OnInitialized()
    {
        TextBlockScrollViewer.ScrollChanged += TextBlockScrollViewerOnScrollChanged;
    }

    private void TextBlockScrollViewerOnScrollChanged(object? sender, ScrollChangedEventArgs e)
    {
        var distanceToEnd = TextBlockScrollViewer.ScrollBarMaximum.Y - TextBlockScrollViewer.Offset.Y;
        var distanceToEndBeforeScrollChange = distanceToEnd - e.ExtentDelta.Y;
        if (e.ExtentDelta.Y != 0 && distanceToEndBeforeScrollChange == 0)
            TextBlockScrollViewer.ScrollToEnd();
    }
}