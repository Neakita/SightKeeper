using SightKeeper.Application.Models;
using SightKeeper.Domain.Model.Common;
using SightKeeper.Domain.Model.Detector;
using SightKeeper.Infrastructure.Data;
using SightKeeper.Tests.Common;

namespace SightKeeper.Application.Tests.Models;

public sealed class DetectorModelsProviderTests : DbRelatedTests
{
	private IModelsProvider<DetectorModel> DetectorModelsProvider => new DetectorModelsProvider(DbContextFactory);

	[Fact]
	public void ShouldGetSomeDetectorModels()
	{
		DetectorModel testDetectorModel = new("test detector model", new Resolution());
		using AppDbContext dbContext = DbContextFactory.CreateDbContext();
		dbContext.DetectorModels.Add(testDetectorModel);
		dbContext.SaveChanges();

		IEnumerable<DetectorModel> detectorModels = DetectorModelsProvider.Models;

		detectorModels.Should().NotBeEmpty();
	}
}