using Pure.DI;
using SightKeeper.Application;
using SightKeeper.Application.DataSets.Creating;
using SightKeeper.Application.ImageSets.Creating;
using SightKeeper.Data.DataSets.Classifier;
using SightKeeper.Data.ImageSets;
using SightKeeper.Data.Services;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Classifier;
using SightKeeper.Domain.Images;

namespace SightKeeper.Avalonia;

public sealed class PersistenceComposition
{
	private void Setup() => DI.Setup(nameof(PersistenceComposition), CompositionKind.Internal)

		.Bind<WriteRepository<ImageSet>>()
		.Bind<ReadRepository<ImageSet>>()
		.Bind<ObservableRepository<ImageSet>>()
		.To<AppDataImageSetsRepository>()

		.Bind<WriteRepository<DataSet>>()
		.Bind<ReadRepository<DataSet>>()
		.Bind<ObservableRepository<DataSet>>()
		.To<AppDataDataSetsRepository>()

		.Bind<ImageSetFactory<ImageSet>>()
		.To<WrappedImageSetFactory>()

		.Bind<DataSetFactory<ClassifierDataSet>>()
		.To<WrappingClassifierDataSetFactory>();
}