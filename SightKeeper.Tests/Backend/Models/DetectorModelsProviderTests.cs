using FluentAssertions;
using SightKeeper.Backend.Models;
using SightKeeper.DAL;
using SightKeeper.DAL.Domain.Common;
using SightKeeper.DAL.Domain.Detector;

namespace SightKeeper.Tests.Backend.Models;

public sealed class DetectorModelsProviderTests : DbRelatedTests
{
	private IModelsProvider<DetectorModel> DetectorModelsProvider => new DetectorModelsProvider(DbProvider);

	[Fact]
	public void ShouldGetSomeDetectorModels()
	{
		DetectorModel testDetectorModel = new("test detector model", new Resolution());
		using AppDbContext dbContext = DbProvider.NewContext;
		dbContext.DetectorModels.Add(testDetectorModel);
		dbContext.SaveChanges();

		IEnumerable<DetectorModel> detectorModels = DetectorModelsProvider.Models;

		detectorModels.Should().NotBeEmpty();
	}
}