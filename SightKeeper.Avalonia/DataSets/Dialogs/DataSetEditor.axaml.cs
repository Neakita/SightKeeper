using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace SightKeeper.Avalonia.DataSets.Dialogs;

public sealed partial class DataSetEditor : UserControl/*, IViewFor<IDataSetEditorViewModel>, IViewFor<DataSetEditingViewModel>, IViewFor<DataSetCreatingViewModel>*/
{
    // IDataSetEditorViewModel? IViewFor<IDataSetEditorViewModel>.ViewModel { get; set; }
    // DataSetEditingViewModel? IViewFor<DataSetEditingViewModel>.ViewModel { get; set; }
    //
    // DataSetCreatingViewModel? IViewFor<DataSetCreatingViewModel>.ViewModel { get; set; }
    
    public DataSetEditor()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    // object? IViewFor.ViewModel
    // {
    //     get => DataContext;
    //     set => DataContext = (IDataSetEditorViewModel?)value;
    // }
}