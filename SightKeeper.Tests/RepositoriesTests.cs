using Microsoft.EntityFrameworkCore;
using SightKeeper.DAL.Members.Common;
using SightKeeper.DAL.Members.Detector;
using SightKeeper.DAL.Repositories;
using SightKeeper.Services;

namespace SightKeeper.Tests;

public sealed class RepositoriesTests
{
	[Fact]
	public void ShouldGetDetectorModelRepository()
	{
		DeleteDatabase();
		ServiceManager.GetService<IRepository<DetectorModel>>();
		Assert.True(true);
	}

	[Fact]
	public void DetectorModelRepositoryShouldStoreAndReturnDetectorModel()
	{
		// assign
		DeleteDatabase();
		var repository = ServiceManager.GetService<IRepository<DetectorModel>>();
		var model = new DetectorModel("untitled", new Resolution());

		// act
		repository.Add(model);
		DetectorModel? modelFromRepository = repository.Items.FirstOrDefault();

		// assert
		Assert.Equal(model, modelFromRepository);
	}
	
	[Fact]
	public void DetectorModelRepositoryShouldStoreAndReturnDetectorModelFromDifferentContext()
	{
		// assign
		DeleteDatabase();
		var repository = ServiceManager.GetService<IRepository<DetectorModel>>();
		var model = new DetectorModel("untitled", new Resolution());

		// act
		repository.Add(model);
		repository = ServiceManager.GetService<IRepository<DetectorModel>>();
		DetectorModel? modelFromRepository = repository.Items.FirstOrDefault();

		// assert
		Assert.True(model.EqualsById(modelFromRepository));
	}

	private static void DeleteDatabase() => ServiceManager.GetService<DbContext>().Database.EnsureDeleted();
}
