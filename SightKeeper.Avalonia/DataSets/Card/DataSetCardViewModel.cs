using System.Linq;
using System.Windows.Input;
using SightKeeper.Avalonia.Annotation.Images;
using SightKeeper.Domain.DataSets;

namespace SightKeeper.Avalonia.DataSets.Card;

internal sealed class DataSetCardViewModel : ViewModel, DataSetCardDataContext
{
	public string Name => _dataSet.Name;
	public ImageDataContext? Image { get; }
	public ICommand EditCommand { get; }
	public ICommand ExportCommand { get; }
	public ICommand DeleteCommand { get; }

	public DataSetCardViewModel(DataSet dataSet, ICommand editCommand, ICommand exportCommand, ICommand deleteCommand, ImageLoader imageLoader)
	{
		_dataSet = dataSet;
		var image = dataSet.AssetsLibrary.Images.FirstOrDefault();
		if (image != null)
			Image = new ImageViewModel(imageLoader, image);
		EditCommand = editCommand;
		ExportCommand = exportCommand;
		DeleteCommand = deleteCommand;
	}

	private readonly DataSet _dataSet;
}