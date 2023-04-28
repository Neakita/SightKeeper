using SightKeeper.Data;
using SightKeeper.Domain.Model.Common;
using SightKeeper.Domain.Model.Detector;
using SightKeeper.Tests.Common;

namespace SightKeeper.Infrastructure.Data.Tests;

public sealed class ProfilesTests : DbRelatedTests
{
	[Fact]
	public void ShouldCreateProfile()
	{
		using AppDbContext dbContext = DbContextFactory.CreateDbContext();
		Profile profile = new("Test profile", new DetectorModel("Detector"));

		dbContext.Profiles.Add(profile);
		dbContext.SaveChanges();

		dbContext.Profiles.Should().Contain(profile);
	}

	[Fact]
	public void ShouldNotDeleteModelOnProfileDelete()
	{
		using AppDbContext dbContext = DbContextFactory.CreateDbContext();
		DetectorModel model = new("Test model");
		Profile profile = new("Test profile", model);
		dbContext.Profiles.Add(profile);
		dbContext.SaveChanges();

		dbContext.Profiles.Should().Contain(profile);
		dbContext.DetectorModels.Should().Contain(model);

		dbContext.Profiles.Remove(profile);
		dbContext.SaveChanges(); 

		dbContext.Profiles.Should().BeEmpty();
		dbContext.DetectorModels.Should().Contain(model);
	}
}