using SightKeeper.Avalonia.Annotation.Images;
using SightKeeper.Avalonia.DataSets.Commands;
using SightKeeper.Avalonia.Extensions;
using SightKeeper.Domain.DataSets;

namespace SightKeeper.Avalonia.DataSets.Card;

internal sealed class DataSetCardViewModelFactory(
	EditDataSetCommand editDataSetCommand,
	ExportDataSetCommand exportDataSetCommand,
	DeleteDataSetCommand deleteDataSetCommand,
	ImageLoader imageLoader)
{
	public DataSetCardViewModel CreateDataSetCardViewModel(DataSet dataSet)
	{
		return new DataSetCardViewModel(
			dataSet,
			editDataSetCommand.WithParameter(dataSet),
			exportDataSetCommand.WithParameter(dataSet),
			deleteDataSetCommand.WithParameter(dataSet),
			imageLoader);
	}
}