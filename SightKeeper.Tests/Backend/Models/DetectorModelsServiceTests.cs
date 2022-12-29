using FluentAssertions;
using SightKeeper.Backend.Models;
using SightKeeper.DAL;
using SightKeeper.DAL.Domain.Common;
using SightKeeper.DAL.Domain.Detector;

namespace SightKeeper.Tests.Backend.Models;

public sealed class DetectorModelsServiceTests : DbRelatedTests
{
	private DetectorModelsService Service => new(DbProvider);

	[Fact]
	public void ShouldCreateDetectorModel()
	{
		// arrange
		const string testModelName = "Test model";

		// act
		Service.Create(testModelName, 320, 320);

		// assert
		using AppDbContext dbContext = DbProvider.NewContext;
		dbContext.DetectorModels.Should().Contain(model => model.Name == testModelName);
	}

	[Fact]
	public void ShouldDeleteDetectorModel()
	{
		// arrange
		const string testModelName = "Test model";
		DetectorModel detectorModel = new(testModelName, new Resolution());
		using AppDbContext dbContext = DbProvider.NewContext;
		dbContext.DetectorModels.Add(detectorModel);
		dbContext.SaveChanges();

		// act
		Service.Delete(detectorModel);

		// assert
		dbContext.DetectorModels.Should().NotContain(model => model.Name == testModelName);
	}
}