using FluentAssertions;
using SightKeeper.DAL;
using SightKeeper.DAL.Domain.Common;
using SightKeeper.DAL.Domain.Detector;

namespace SightKeeper.Tests.DAL;

public sealed class DetectorModelTests : DbRelatedTests
{
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
		// arrange
		using AppDbContext dbContext = DbProvider.NewContext;
		DetectorModel model = TestDetectorModel;
		DetectorScreenshot iScreenshot = new();
		ItemClass itemClass = new("class");
		DetectorItem item = new(itemClass, new BoundingBox(0, 0, 1, 1));
		iScreenshot.Items.Add(item);
		model.DetectorScreenshots.Add(iScreenshot);
		
		// act
		dbContext.Add(model);
		dbContext.SaveChanges();
		
		// assert
		dbContext.DetectorModels.Should().Contain(model);
		dbContext.ItemClasses.Should().Contain(itemClass);
		dbContext.DetectorItems.Should().Contain(item);
		dbContext.DetectorScreenshots.Should().Contain(iScreenshot);
	}

	[Fact]
	public void DeletingModelWithScreenshotShouldCascadeDeleteScreenshots()
	{
		using AppDbContext dbContext = DbProvider.NewContext;
		
	}


	private static DetectorModel TestDetectorModel => new("TestDetectorModel");
}