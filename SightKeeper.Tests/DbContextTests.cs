using SightKeeper.Backend.Data;
using SightKeeper.Backend.Data.Members;
using SightKeeper.Backend.Data.Members.Detector;

namespace SightKeeper.Tests;

public class DbContextTests
{
	private const string TestDbName = "test.db";

	[Fact]
	public void CanCreateDbContext()
	{
		// assign
		using var dbContext = new AppDbContext(TestDbName);
		dbContext.Database.EnsureDeleted();
		
		// act
		bool created = dbContext.Database.EnsureCreated();
		
		// assert
		Assert.True(created);
		
		// clean-up
		dbContext.Database.EnsureDeleted();
	}

	[Fact]
	public void DeletingModelWillNotDeleteScreenshot()
	{
		// assign
		var detectorModel = new DetectorModel();
		var screenshot = new DetectorScreenshot {Model = detectorModel};
		detectorModel.DetectorScreenshots.Add(screenshot);
		using (var dbContext = new AppDbContext(TestDbName))
		{
			dbContext.Database.EnsureDeleted();
			dbContext.Database.EnsureCreated();
			dbContext.DetectorModels.Add(detectorModel);
			dbContext.DetectorScreenshots.Add(screenshot);
			dbContext.SaveChanges();
			Assert.NotEmpty(dbContext.DetectorModels);
			Assert.NotEmpty(dbContext.DetectorScreenshots);
		}
		
		using (var dbContext = new AppDbContext(TestDbName))
		{
			// act
			dbContext.DetectorModels.Remove(detectorModel);
			dbContext.SaveChanges();
			
			// assert
			Assert.Empty(dbContext.DetectorModels);
			Assert.NotEmpty(dbContext.DetectorScreenshots);
			
			// clean-up
			dbContext.Database.EnsureDeleted();
		}
	}

	[Fact]
	public void DeletingScreenshotWillNotDeleteDetectorModel()
	{
		// assign
		var detectorModel = new DetectorModel();
		var screenshot = new DetectorScreenshot {Model = detectorModel};
		detectorModel.DetectorScreenshots.Add(screenshot);
		using (var dbContext = new AppDbContext(TestDbName))
		{
			dbContext.Database.EnsureDeleted();
			dbContext.Database.EnsureCreated();
			dbContext.DetectorModels.Add(detectorModel);
			dbContext.DetectorScreenshots.Add(screenshot);
			dbContext.SaveChanges();
			Assert.NotEmpty(dbContext.DetectorModels);
			Assert.NotEmpty(dbContext.DetectorScreenshots);
		}
		
		using (var dbContext = new AppDbContext(TestDbName))
		{
			// act
			dbContext.DetectorScreenshots.Remove(screenshot);
			dbContext.SaveChanges();
			
			// assert
			Assert.NotEmpty(dbContext.DetectorModels);
			Assert.Empty(dbContext.DetectorScreenshots);
			
			// clean-up
			dbContext.Database.EnsureDeleted();
		}
	}

	[Fact]
	public void DeletingScreenshotWillDeleteDetectorItem()
	{
		// assign
		var detectorModel = new DetectorModel();
		var screenshot = new DetectorScreenshot {Model = detectorModel};
		var detectorItem = new DetectorItem
		{
			Screenshot = screenshot,
			ItemClass = new ItemClass(),
			BoundingBox = new BoundingBox()
		};

		detectorModel.DetectorScreenshots.Add(screenshot);
		screenshot.Items.Add(detectorItem);
		using (var dbContext = new AppDbContext(TestDbName))
		{
			dbContext.Database.EnsureDeleted();
			dbContext.Database.EnsureCreated();
			dbContext.DetectorModels.Add(detectorModel);
			dbContext.DetectorScreenshots.Add(screenshot);
			dbContext.DetectorScreenshots.Add(screenshot);
			dbContext.SaveChanges();
			Assert.NotEmpty(dbContext.DetectorScreenshots);
			Assert.NotEmpty(dbContext.DetectorItems);
		}
		
		using (var dbContext = new AppDbContext(TestDbName))
		{
			// act
			dbContext.DetectorScreenshots.Remove(screenshot);
			dbContext.SaveChanges();
			
			// assert
			Assert.Empty(dbContext.DetectorItems);

			// clean-up
			dbContext.Database.EnsureDeleted();
		}
	}

	[Fact]
	public void DeletingDetectorItemWillNotDeleteScreenshot()
	{
		// assign
		var detectorModel = new DetectorModel();
		var screenshot = new DetectorScreenshot {Model = detectorModel};
		var detectorItem = new DetectorItem
		{
			Screenshot = screenshot,
			ItemClass = new ItemClass(),
			BoundingBox = new BoundingBox()
		};

		detectorModel.DetectorScreenshots.Add(screenshot);
		screenshot.Items.Add(detectorItem);
		using (var dbContext = new AppDbContext(TestDbName))
		{
			dbContext.Database.EnsureDeleted();
			dbContext.Database.EnsureCreated();
			dbContext.DetectorModels.Add(detectorModel);
			dbContext.DetectorScreenshots.Add(screenshot);
			dbContext.DetectorScreenshots.Add(screenshot);
			dbContext.SaveChanges();
			Assert.NotEmpty(dbContext.DetectorScreenshots);
			Assert.NotEmpty(dbContext.DetectorItems);
		}
		
		using (var dbContext = new AppDbContext(TestDbName))
		{
			// act
			dbContext.DetectorItems.Remove(detectorItem);
			dbContext.SaveChanges();
			
			// assert
			Assert.NotEmpty(dbContext.DetectorScreenshots);

			// clean-up
			dbContext.Database.EnsureDeleted();
		}
	}

	[Fact]
	public void DeletingItemClassWillDeleteDetectorItem()
	{
		// assign
		var detectorModel = new DetectorModel();
		var screenshot = new DetectorScreenshot {Model = detectorModel};
		var itemClass = new ItemClass();
		var detectorItem = new DetectorItem
		{
			Screenshot = screenshot,
			ItemClass = itemClass,
			BoundingBox = new BoundingBox()
		};

		detectorModel.DetectorScreenshots.Add(screenshot);
		screenshot.Items.Add(detectorItem);
		using (var dbContext = new AppDbContext(TestDbName))
		{
			dbContext.Database.EnsureDeleted();
			dbContext.Database.EnsureCreated();
			dbContext.DetectorModels.Add(detectorModel);
			dbContext.DetectorScreenshots.Add(screenshot);
			dbContext.DetectorScreenshots.Add(screenshot);
			dbContext.SaveChanges();
			Assert.NotEmpty(dbContext.DetectorScreenshots);
			Assert.NotEmpty(dbContext.DetectorItems);
		}
		
		using (var dbContext = new AppDbContext(TestDbName))
		{
			// act
			dbContext.ItemClasses.Remove(itemClass);
			dbContext.SaveChanges();
			
			// assert
			Assert.Empty(dbContext.DetectorItems);

			// clean-up
			dbContext.Database.EnsureDeleted();
		}
	}

	[Fact]
	public void DeletingDetectorItemWillNotDeleteItemClass()
	{
		// assign
		var detectorModel = new DetectorModel();
		var screenshot = new DetectorScreenshot {Model = detectorModel};
		var detectorItem = new DetectorItem
		{
			Screenshot = screenshot,
			ItemClass = new ItemClass(),
			BoundingBox = new BoundingBox()
		};

		detectorModel.DetectorScreenshots.Add(screenshot);
		screenshot.Items.Add(detectorItem);
		using (var dbContext = new AppDbContext(TestDbName))
		{
			dbContext.Database.EnsureDeleted();
			dbContext.Database.EnsureCreated();
			dbContext.DetectorModels.Add(detectorModel);
			dbContext.DetectorScreenshots.Add(screenshot);
			dbContext.DetectorScreenshots.Add(screenshot);
			dbContext.SaveChanges();
			Assert.NotEmpty(dbContext.DetectorScreenshots);
			Assert.NotEmpty(dbContext.DetectorItems);
		}
		
		using (var dbContext = new AppDbContext(TestDbName))
		{
			// act
			dbContext.DetectorItems.Remove(detectorItem);
			dbContext.SaveChanges();
			
			// assert
			Assert.NotEmpty(dbContext.ItemClasses);

			// clean-up
			dbContext.Database.EnsureDeleted();
		}
	}
}