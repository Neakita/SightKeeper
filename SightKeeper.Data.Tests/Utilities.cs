using NSubstitute;
using SightKeeper.Application.ScreenCapturing;
using SightKeeper.Data.Model.Images;
using SightKeeper.Data.Services;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.Tests;

internal static class Utilities
{
	public static void AddImageSetToAppData(DomainImageSet set, AppDataAccess appDataAccess)
	{
		AppDataImageSetsRepository repository = new()
		{
			AppDataLock = new Lock(),
			AppDataAccess = appDataAccess,
			ChangeListener = Substitute.For<ChangeListener>(),
			ImageDataAccess = Substitute.For<WriteImageDataAccess>()
		};
		repository.Add(set);
	}

	public static void AddDataSetToAppData(DataSet set, AppDataAccess appDataAccess)
	{
		AppDataDataSetsRepository repository = new(
			appDataAccess,
			Substitute.For<ChangeListener>(),
			new Lock());
		repository.Add(set);
	}

	public static ImageSet CreateImageSet()
	{
		var factory = new StorableImageSetFactory(Substitute.For<ChangeListener>(), new Lock());
		return factory.CreateImageSet();
	}

	public static ImageSet Persist(this ImageSet set)
	{
		PersistenceBootstrapper.Setup(Substitute.For<ChangeListener>(), new Lock());
		AppDataAccess appDataAccess = new();
		AppDataImageSetsRepository repository = new()
		{
			AppDataLock = new Lock(),
			AppDataAccess = appDataAccess,
			ChangeListener = Substitute.For<ChangeListener>(),
			ImageDataAccess = Substitute.For<WriteImageDataAccess>()
		};
		repository.Add(set);
		appDataAccess.Save();
		appDataAccess.Load();
		return repository.Items.Single();
	}
}