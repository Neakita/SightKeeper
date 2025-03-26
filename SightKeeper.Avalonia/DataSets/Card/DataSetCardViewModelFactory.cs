using System.Windows.Input;
using SightKeeper.Avalonia.Annotation.Images;
using SightKeeper.Avalonia.DataSets.Commands;
using SightKeeper.Avalonia.Extensions;
using SightKeeper.Domain.DataSets;

namespace SightKeeper.Avalonia.DataSets.Card;

internal sealed class DataSetCardViewModelFactory
{
	public DataSetCardViewModelFactory(
		EditDataSetCommandFactory editDataSetCommandFactory,
		DeleteDataSetCommandFactory deleteDataSetCommandFactory,
		ImageLoader imageLoader)
	{
		_editDataSetCommand = editDataSetCommandFactory.CreateCommand();
		_deleteDataSetCommand = deleteDataSetCommandFactory.CreateCommand();
		_imageLoader = imageLoader;
	}

	public DataSetCardViewModel CreateDataSetCardViewModel(DataSet dataSet)
	{
		var editCommand = _editDataSetCommand.WithParameter(dataSet);
		var deleteCommand = _deleteDataSetCommand.WithParameter(dataSet);
		return new DataSetCardViewModel(dataSet, editCommand, deleteCommand, _imageLoader);
	}

	private readonly ICommand _editDataSetCommand;
	private readonly ICommand _deleteDataSetCommand;
	private readonly ImageLoader _imageLoader;
}