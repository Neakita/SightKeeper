using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using SightKeeper.Avalonia.DataSets.Dialogs.Tags;
using SightKeeper.Avalonia.Dialogs;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Avalonia.DataSets.Dialogs;

internal abstract partial class DataSetDialogViewModel : DialogViewModel<bool>, DataSetDialogDataContext
{
	public DataSetEditorViewModel DataSetEditor { get; }
	public abstract TagsEditorDataContext TagsEditor { get; }
	public virtual DataSetTypePickerViewModel? TypePicker => null;

	DataSetEditorDataContext DataSetDialogDataContext.DataSetEditor => DataSetEditor;
	TagsEditorDataContext DataSetDialogDataContext.TagsEditor => TagsEditor;
	ICommand DataSetDialogDataContext.ApplyCommand => ApplyCommand;
	ICommand DataSetDialogDataContext.CloseCommand => CloseCommand;

	protected DataSetDialogViewModel()
	{
		DataSetEditor = new DataSetEditorViewModel();
	}

	public DataSetDialogViewModel(DataSet<Tag, Asset> dataSet)
	{
		DataSetEditor = new DataSetEditorViewModel(dataSet);
	}

	protected sealed override bool DefaultResult => false;

	[RelayCommand]
	private void Apply()
	{
		Return(true);
	}
}