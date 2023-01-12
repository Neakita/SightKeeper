using FluentAssertions;
using SightKeeper.Domain.Common;
using SightKeeper.Domain.Detector;
using SightKeeper.Persistance;

namespace SightKeeper.Tests.DAL;

public sealed class ProfilesTests : DbRelatedTests
{
	[Fact]
	public void ShouldCreateProfile()
	{
		using AppDbContext dbContext = DbProvider.NewContext;
		Profile profile = new("Test profile");

		dbContext.Profiles.Add(profile);
		dbContext.SaveChanges();

		dbContext.Profiles.Should().Contain(profile);
	}

	[Fact]
	public void ShouldNotDeleteModelOnProfileDelete()
	{
		using AppDbContext dbContext = DbProvider.NewContext;
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

		using AppDbContext dbContext = DbProvider.NewContext;
		dbContext.Profiles.Add(profile);
		dbContext.SaveChanges();

		dbContext.ProfileComponents.Should().Contain(component);

		dbContext.Profiles.Remove(profile);
		dbContext.SaveChanges();

		dbContext.Profiles.Should().BeEmpty();
		dbContext.ProfileComponents.Should().BeEmpty();
	}
}