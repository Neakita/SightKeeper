using FluentAssertions;
using SightKeeper.DAL;
using SightKeeper.DAL.Domain.Common;
using SightKeeper.DAL.Domain.Detector;

namespace SightKeeper.Tests.DAL;

public sealed class DetectorModelTests : DbRelatedTests
{
	private static DetectorModel TestDetectorModel => new("TestDetectorModel");

	[Fact]
	public void ShouldAddDetectorModel()
	{
		// arrange
		using AppDbContext dbContext = DbProvider.NewContext;
		DetectorModel testModel = TestDetectorModel;

		// act
		dbContext.Add(testModel);
		dbContext.SaveChanges();

		// assert
		dbContext.DetectorModels.Should().Contain(testModel);
	}

	[Fact]
	public void AddingModelWithScreenshotShouldAddScreenshot()
	{
		using AppDbContext dbContext = DbProvider.NewContext;
		DetectorModel model = TestDetectorModel;
		DetectorScreenshot screenshot = new(model);
		ItemClass itemClass = new("class");
		DetectorItem item = new(itemClass, new BoundingBox(0, 0, 1, 1));
		screenshot.Items.Add(item);
		model.DetectorScreenshots.Add(screenshot);
		
		dbContext.DetectorModels.Add(model);
		dbContext.SaveChanges();

		dbContext.DetectorModels.Should().Contain(model);
		dbContext.DetectorScreenshots.Should().Contain(screenshot);
		dbContext.DetectorItems.Should().Contain(item);
		dbContext.ItemClasses.ToList().Should().Contain(itemClass);
	}

	[Fact]
	public void ShouldDeleteWithScreenshots()
	{
		using AppDbContext dbContext = DbProvider.NewContext;
		DetectorModel model = new("Test model");
		DetectorScreenshot screenshot = new(model);
		model.DetectorScreenshots.Add(screenshot);

		dbContext.DetectorModels.Add(model);
		dbContext.SaveChanges();

		dbContext.DetectorModels.Should().Contain(model);
		dbContext.DetectorScreenshots.Should().Contain(screenshot);

		dbContext.DetectorModels.Remove(model);
		dbContext.SaveChanges();

		dbContext.DetectorModels.Should().BeEmpty();
		dbContext.DetectorScreenshots.Should().BeEmpty();
	}
}