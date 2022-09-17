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
		ServiceManager.GetService<IRepository<DetectorModel>>();
		Assert.True(true);
	}

	[Fact]
	public void DetectorModelRepositoryShouldStoreAndReturnDetectorModel()
	{
		// assign
		var repository = ServiceManager.GetService<IRepository<DetectorModel>>();
		var model = new DetectorModel("untitled", new Resolution());

		// act
		repository.Add(model);
		DetectorModel modelFromRepository = repository.Items.Single();
		
		// assert
		Assert.Equal(model, modelFromRepository);
		
		// clean-up
		repository.Clear();
	}
}
