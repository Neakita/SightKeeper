﻿using FluentAssertions;
using SightKeeper.DAL;
using SightKeeper.DAL.Domain.Common;
using SightKeeper.DAL.Domain.Detector;

namespace SightKeeper.Tests.DAL;

public sealed class ItemClassesTests : DbRelatedTests
{
	[Fact]
	public void ShouldNotDeleteItemClassesOnItemDelete()
	{
		using AppDbContext dbContext = DbProvider.NewContext;
		ItemClass itemClass = new("Test item class");
		DetectorItem item = new(itemClass, new BoundingBox(0, 0, 1, 1));
		DetectorModel model = new("Test model");
		DetectorScreenshot screenshot = new(model);
		screenshot.Items.Add(item);
		model.DetectorScreenshots.Add(screenshot);
		dbContext.DetectorModels.Add(model);
		dbContext.SaveChanges();

		dbContext.ItemClasses.Should().Contain(itemClass);
		dbContext.DetectorItems.Should().Contain(item);

		dbContext.DetectorItems.Remove(item);
		dbContext.SaveChanges();

		dbContext.ItemClasses.Should().Contain(itemClass);
		dbContext.DetectorItems.Should().BeEmpty();
	}
}
