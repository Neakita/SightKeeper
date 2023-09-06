using System.Collections.ObjectModel;

namespace SightKeeper.Avalonia.ViewModels.Annotating;

public sealed class SelectedScreenshotViewModel : ValueViewModel<ScreenshotViewModel?>
{
    public int? SelectedScreenshotIndex
    {
        get => _selectedScreenshotIndex;
        set => SetProperty(ref _selectedScreenshotIndex, value);
    }

    public ObservableCollection<DrawerItem> Items { get; } = new();

    public SelectedScreenshotViewModel() : base(null)
    {
    }

    protected override void OnValueChanged(ScreenshotViewModel? newValue)
    {
        Items.Clear();
    }
    
    private int? _selectedScreenshotIndex;
}