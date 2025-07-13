using SightKeeper.Application;
using SightKeeper.Application.DataSets.Creating;
using SightKeeper.Application.ImageSets.Creating;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Classifier;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data;

public sealed class PersistenceServices
{
	public required WriteRepository<ImageSet> WriteImageSetRepository { get; init; }
	public required ReadRepository<ImageSet> ReadImageSetRepository { get; init; }
	public required ObservableRepository<ImageSet> ObservableImageSetRepository { get; init; }

	public required WriteRepository<DataSet> WriteDataSetRepository { get; init; }
	public required ReadRepository<DataSet> ReadDataSetRepository { get; init; }
	public required ObservableRepository<DataSet> ObservableDataSetRepository { get; init; }
	
	public required ImageSetFactory<ImageSet> ImageSetFactory { get; init; }
	public required DataSetFactory<ClassifierDataSet> ClassifierDataSetFactory { get; init; }
}