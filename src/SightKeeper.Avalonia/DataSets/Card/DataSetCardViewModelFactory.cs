using SightKeeper.Avalonia.Annotation.Images;
using SightKeeper.Avalonia.DataSets.Commands;
using SightKeeper.Avalonia.Extensions;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Avalonia.DataSets.Card;

internal sealed class DataSetCardViewModelFactory(
	EditDataSetCommand editDataSetCommand,
	ExportDataSetCommand exportDataSetCommand,
	DeleteDataSetCommand deleteDataSetCommand,
	WriteableBitmapImageLoader imageLoader)
{
	public DataSetCardViewModel CreateDataSetCardViewModel(DataSet<Tag, Asset> dataSet)
	{
		return new DataSetCardViewModel(
			dataSet,
			editDataSetCommand.WithParameter(dataSet),
			exportDataSetCommand.WithParameter(dataSet),
			deleteDataSetCommand.WithParameter(dataSet),
			imageLoader);
	}
}