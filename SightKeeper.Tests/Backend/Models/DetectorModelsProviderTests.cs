using FluentAssertions;
using SightKeeper.Abstractions.Domain;
using SightKeeper.Backend.Models;
using SightKeeper.DAL;
using SightKeeper.DAL.Domain.Common;
using SightKeeper.DAL.Domain.Detector;

namespace SightKeeper.Tests.Backend.Models;

public sealed class DetectorModelsProviderTests : DbRelatedTests
{
	[Fact]
	public void ShouldGetSomeDetectorModels()
	{
		DetectorModel testDetectorModel = new("test detector model", new Resolution());
		using AppDbContext dbContext = DbProvider.NewContext;
		dbContext.DetectorModels.Add(testDetectorModel);
		dbContext.SaveChanges();
		
		IEnumerable<IDetectorModel> detectorModels = DetectorModelsProvider.Models;
		
		detectorModels.Should().NotBeEmpty();
	}
	
	private IModelsProvider<IDetectorModel> DetectorModelsProvider => new DetectorModelsProvider(DbProvider);
}