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
		using AppDbContext dbContext = DbContextFactory.CreateDbContext();
		ItemClass itemClass = new("Test item class");
		DetectorItem item = new(itemClass, new BoundingBox());
		DetectorModel model = new("Test model");
		DetectorAsset asset = new(model, new Screenshot(new Image(Array.Empty<byte>())));
		asset.AddItem(item);
		model.Assets.Add(asset);
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
