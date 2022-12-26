using FluentAssertions;
using SightKeeper.Abstractions;
using SightKeeper.Abstractions.Domain;
using SightKeeper.Backend.Models;
using SightKeeper.DAL;
using SightKeeper.DAL.Domain.Common;
using SightKeeper.DAL.Domain.Detector;

namespace SightKeeper.Tests.Backend.Models;

public sealed class DetectorModelsProviderTests
{
	[Fact]
	public void ShouldGetSomeDetectorModels()
	{
		// arrange
		DetectorModel testDetectorModel = new("test detector model", new Resolution());
		using (IAppDbContext dbContext = Helper.DbProvider.NewContext)
		{
			dbContext.DetectorModels.Add(testDetectorModel);
			dbContext.SaveChanges();
		}
		
		// act
		IEnumerable<IDetectorModel> detectorModels = _detectorModelsProvider.Models;

		// assert
		detectorModels.Should().NotBeEmpty();
		
		// clean-up
		using (IAppDbContext dbContext = Helper.DbProvider.NewContext) dbContext.Clear();
	}
	

	private readonly IModelsProvider<IDetectorModel> _detectorModelsProvider = new DetectorModelsProvider(Helper.DbProvider);

	
}
