using SightKeeper.Domain.Model.Abstract;
using SightKeeper.Domain.Model.Common;
using SightKeeper.Domain.Model.Detector;
using SightKeeper.Tests.Common;

namespace SightKeeper.Data.Tests;

public sealed class ItemClassesTests : DbRelatedTests
{
	[Fact]
	public void ShouldNotDeleteItemClassesOnItemDelete()
	{
		using var dbContext = DbContextFactory.CreateDbContext();
		DetectorModel model = new("Test model");
		var itemClass = model.CreateItemClass("Test item class");
		Screenshot screenshot = new(new Image(Array.Empty<byte>()));
		model.AddScreenshot(screenshot);
		var asset = model.MakeAssetFromScreenshot(screenshot);
		DetectorItem item = new(itemClass, new BoundingBox());
		asset.AddItem(item);
		dbContext.DetectorModels.Add(model);
		dbContext.SaveChanges();

		dbContext.Set<ItemClass>().Should().Contain(itemClass);
		dbContext.Set<DetectorItem>().Should().Contain(item);

		dbContext.Set<DetectorItem>().Remove(item);
		dbContext.SaveChanges();

		dbContext.Set<ItemClass>().Should().Contain(itemClass);
		dbContext.Set<DetectorItem>().Should().BeEmpty();
	}
}
