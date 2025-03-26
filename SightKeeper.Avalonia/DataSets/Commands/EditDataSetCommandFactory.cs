using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using SightKeeper.Application;
using SightKeeper.Application.DataSets;
using SightKeeper.Application.DataSets.Editing;
using SightKeeper.Avalonia.DataSets.Dialogs;
using SightKeeper.Avalonia.Dialogs;
using SightKeeper.Domain.DataSets;

namespace SightKeeper.Avalonia.DataSets.Commands;

internal sealed class EditDataSetCommandFactory
{
	public EditDataSetCommandFactory(ReadDataAccess<DataSet> readDataSetsDataAccess, DialogManager dialogManager, DataSetEditor dataSetEditor)
	{
		_readDataSetsDataAccess = readDataSetsDataAccess;
		_dialogManager = dialogManager;
		_dataSetEditor = dataSetEditor;
	}

	public ICommand CreateCommand()
	{
		return new AsyncRelayCommand<DataSet>(EditDataSetAsync!);
	}

	private readonly ReadDataAccess<DataSet> _readDataSetsDataAccess;
	private readonly DialogManager _dialogManager;
	private readonly DataSetEditor _dataSetEditor;

	private async Task EditDataSetAsync(DataSet dataSet)
	{
		using EditDataSetViewModel dialog = new(dataSet, new DataSetEditorViewModel(new ExistingDataSetDataValidator(dataSet, new DataSetDataValidator(), _readDataSetsDataAccess), dataSet));
		if (await _dialogManager.ShowDialogAsync(dialog))
			_dataSetEditor.Edit(dataSet, dialog.DataSetEditor, dialog.TagsEditor.Tags);
	}
}