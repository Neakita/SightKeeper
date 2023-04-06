using FluentAssertions;
using SightKeeper.Domain.Model.Common;
using SightKeeper.Domain.Model.Detector;
using SightKeeper.Infrastructure.Data;
using SightKeeper.Tests.Common;

namespace SightKeeper.Infrastructure.Services.Tests;

public sealed class ModelScreenshoterImplementationTests
{
	[Fact]
	public void ShouldLoadImagesForSelectedModel()
	{
		TestDbContextFactory testDbContextFactory = new();
		using (AppDbContext arrangeContext = testDbContextFactory.CreateDbContext())
		{
			DetectorModel model = new("Test model");
			arrangeContext.Add(model);
			model.DetectorScreenshots.Add(new DetectorScreenshot(model, new Image(Array.Empty<byte>())));
			arrangeContext.SaveChanges();
		}
		OnShootModelScreenshoter screenshoter = new(null!, null!, testDbContextFactory);
		
		using AppDbContext dbContext = testDbContextFactory.CreateDbContext();
		DetectorModel modelFromDb = dbContext.DetectorModels.Single();
		try
		{
			screenshoter.Model = modelFromDb;
		}
		catch
		{
			// ignored
		}
		modelFromDb.DetectorScreenshots.Should().ContainSingle();
		modelFromDb.DetectorScreenshots.Single().Image.Should().NotBeNull();
	}
}
