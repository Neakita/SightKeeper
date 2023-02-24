using FluentAssertions;
using SightKeeper.Backend.Models;
using SightKeeper.Domain.Model.Common;
using SightKeeper.Domain.Model.Detector;
using SightKeeper.Persistance;

namespace SightKeeper.Tests.Backend.Models;

public sealed class GenericModelsServiceTests : DbRelatedTests
{
	private GenericModelsService<DetectorModel> Service => new(new DetectorModelsFactory(), DbProvider);

	[Fact]
	public void ShouldCreateDetectorModel()
	{
		// arrange
		const string testModelName = "Test model";

		// act
		Service.Create(testModelName, new Resolution());

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