using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using SightKeeper.Application;
using SightKeeper.Application.DataSets.Editing;
using SightKeeper.Avalonia.DataSets.Dialogs;
using SightKeeper.Avalonia.Dialogs;
using SightKeeper.Domain.DataSets;

namespace SightKeeper.Avalonia.DataSets.Commands;

internal sealed class EditDataSetCommandFactory
{
	public EditDataSetCommandFactory(ReadRepository<DataSet> readDataSetsRepository, DialogManager dialogManager, DataSetEditor dataSetEditor)
	{
		_readDataSetsRepository = readDataSetsRepository;
		_dialogManager = dialogManager;
		_dataSetEditor = dataSetEditor;
	}

	public ICommand CreateCommand()
	{
		return new AsyncRelayCommand<DataSet>(EditDataSetAsync!);
	}

	private readonly ReadRepository<DataSet> _readDataSetsRepository;
	private readonly DialogManager _dialogManager;
	private readonly DataSetEditor _dataSetEditor;

	private async Task EditDataSetAsync(DataSet dataSet)
	{
		EditDataSetViewModel dialog = new(dataSet);
		if (await _dialogManager.ShowDialogAsync(dialog))
			_dataSetEditor.Edit(dataSet, dialog);
	}
}