using Avalonia.ReactiveUI;
using SightKeeper.Avalonia.ViewModels.Tabs;

namespace SightKeeper.Avalonia.Views.Tabs;

public sealed partial class TrainingTab : ReactiveUserControl<TrainingViewModel>
{
    public TrainingTab()
    {
        InitializeComponent();
    }
}