using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ReactiveUI;
using SightKeeper.Avalonia.ViewModels.Dialogs;

namespace SightKeeper.Avalonia.Views.Dialogs;

public sealed partial class ModelEditor : UserControl, IViewFor<DataSetDataSetEditorView>
{
    public DataSetDataSetEditorView? ViewModel { get; set; }
    
    public ModelEditor()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    object? IViewFor.ViewModel
    {
        get => ViewModel;
        set => ViewModel = (DataSetDataSetEditorView?)value;
    }
}