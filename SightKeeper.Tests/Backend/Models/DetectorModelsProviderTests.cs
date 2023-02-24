using FluentAssertions;
using SightKeeper.Application.Models;
using SightKeeper.Domain.Model.Common;
using SightKeeper.Domain.Model.Detector;
using SightKeeper.Infrastructure.Data;

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