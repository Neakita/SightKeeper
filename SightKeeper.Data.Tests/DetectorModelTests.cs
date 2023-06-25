using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.Abstract;
using SightKeeper.Domain.Model.Common;
using SightKeeper.Domain.Model.Detector;
using SightKeeper.Tests.Common;

namespace SightKeeper.Data.Tests;

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
		Image image = new(Array.Empty<byte>());
		DetectorAsset asset = new(new Screenshot(image));
		ItemClass itemClass = new("class");
		DetectorItem item = new(itemClass, new BoundingBox(0, 0, 0, 0));
		asset.Items.Add(item);
		model.Assets.Add(asset);
		
		dbContext.DetectorModels.Add(model);
		dbContext.SaveChanges();

		dbContext.DetectorModels.Should().Contain(model);
		dbContext.DetectorScreenshots.Should().Contain(asset);
		dbContext.DetectorItems.Should().Contain(item);
		dbContext.ItemClasses.ToList().Should().Contain(itemClass);
	}

	[Fact]
	public void ShouldDeleteWithScreenshots()
	{
		using AppDbContext dbContext = DbContextFactory.CreateDbContext();
		DetectorModel model = new("Test model");
		Image image = new(Array.Empty<byte>());
		DetectorAsset asset = new(new Screenshot(image));
		model.Assets.Add(asset);

		dbContext.DetectorModels.Add(model);
		dbContext.SaveChanges();

		dbContext.DetectorModels.Should().Contain(model);
		dbContext.DetectorScreenshots.Should().Contain(asset);

		dbContext.DetectorModels.Remove(model);
		dbContext.SaveChanges();

		dbContext.DetectorModels.Should().BeEmpty();
		dbContext.DetectorScreenshots.Should().BeEmpty();
	}

	[Fact]
	public void ShouldNotDeleteConfigOnModelDelete()
	{
		using AppDbContext dbContext = DbContextFactory.CreateDbContext();
		DetectorModel model = new("Test model");
		ModelConfig config = new("Test config", "Test content", ModelType.Detector);
		model.Config = config;
		dbContext.Add(model);
		dbContext.SaveChanges();
		dbContext.ModelConfigs.Should().Contain(config);
		dbContext.Remove(model);
		dbContext.SaveChanges();
		dbContext.ModelConfigs.Should().Contain(config);
	}

	[Fact]
	public void ShouldSetConfigToNullOnConfigDelete()
	{
		using AppDbContext dbContext = DbContextFactory.CreateDbContext();
		DetectorModel newTestModel = new("Test model");
		ModelConfig config = new("Test config", "Test content", ModelType.Detector);
		newTestModel.Config = config;
		dbContext.Add(newTestModel);
		dbContext.SaveChanges();
		dbContext.ModelConfigs.Should().Contain(config);
		dbContext.Remove(config);
		dbContext.SaveChanges();
		DetectorModel? modelFromDb = dbContext.DetectorModels.Find(newTestModel.Id);
		modelFromDb.Should().NotBeNull();
		modelFromDb!.Config.Should().BeNull();
	}
}