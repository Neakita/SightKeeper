using CommunityToolkit.Diagnostics;
using MemoryPack;
using NSubstitute;
using Serilog.Core;
using SightKeeper.Application.ScreenCapturing;
using SightKeeper.Data.Services;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.Tests;

internal static class Utilities
{
	public static AppData Persist(this AppData appData)
	{
		FakeIdRepository<Image> idRepository = new();
		var formatter = new AppDataFormatter(idRepository, new Lock(), Logger.None);
		MemoryPackFormatterProvider.Register(formatter);
		var serializedAppData = MemoryPackSerializer.Serialize(appData);
		var deserializedAppData = MemoryPackSerializer.Deserialize<AppData>(serializedAppData);
		Guard.IsNotNull(deserializedAppData);
		return deserializedAppData;
	}

	public static void AddImageSetToAppData(ImageSet set, AppDataAccess appDataAccess)
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
}