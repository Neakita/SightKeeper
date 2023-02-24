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
		Profile profile = new("Test profile");

		dbContext.Profiles.Add(profile);
		dbContext.SaveChanges();

		dbContext.Profiles.Should().Contain(profile);
	}

	[Fact]
	public void ShouldNotDeleteModelOnProfileDelete()
	{
		using AppDbContext dbContext = DbContextFactory.CreateDbContext();
		Profile profile = new("Test profile");
		DetectorModel model = new("Test model");
		ProfileComponent component = new(profile, model);
		profile.Components.Add(component);
		dbContext.Profiles.Add(profile);
		dbContext.SaveChanges();

		dbContext.Profiles.Should().Contain(profile);
		dbContext.DetectorModels.Should().Contain(model);

		dbContext.Profiles.Remove(profile);
		dbContext.SaveChanges(); 

		dbContext.Profiles.Should().BeEmpty();
		dbContext.DetectorModels.Should().Contain(model);
	}

	[Fact]
	public void ShouldDeleteProfileComponentOnProfileDelete()
	{
		Profile profile = new("Test profile");
		DetectorModel model = new("Test model");
		ProfileComponent component = new(profile, model);
		profile.Components.Add(component);

		using AppDbContext dbContext = DbContextFactory.CreateDbContext();
		dbContext.Profiles.Add(profile);
		dbContext.SaveChanges();

		dbContext.ProfileComponents.Should().Contain(component);

		dbContext.Profiles.Remove(profile);
		dbContext.SaveChanges();

		dbContext.Profiles.Should().BeEmpty();
		dbContext.ProfileComponents.Should().BeEmpty();
	}
}