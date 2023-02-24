using SightKeeper.Domain.Model.Common;
using SightKeeper.Domain.Model.Detector;
using SightKeeper.Tests.Common;

namespace SightKeeper.Infrastructure.Data.Tests;

public sealed class DetectorModelTests : DbRelatedTests
{
	private static DetectorModel TestDetectorModel => new("TestDetectorModel");

	[Fact]
	public void ShouldAddDetectorModel()
	{
		// arrange
		using AppDbContext dbContext = DbContextFactory.CreateDbContext();
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
		using AppDbContext dbContext = DbContextFactory.CreateDbContext();
		DetectorModel model = TestDetectorModel;
		Image image = new(Array.Empty<byte>(), new Resolution());
		DetectorScreenshot screenshot = new(model, image);
		ItemClass itemClass = new("class");
		DetectorItem item = new(itemClass, new BoundingBox(0, 0, 0, 0));
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
		using AppDbContext dbContext = DbContextFactory.CreateDbContext();
		DetectorModel model = new("Test model");
		Image image = new(Array.Empty<byte>(), new Resolution());
		DetectorScreenshot screenshot = new(model, image);
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