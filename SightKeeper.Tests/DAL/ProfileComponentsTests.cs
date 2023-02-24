using FluentAssertions;
using SightKeeper.Domain.Model.Common;
using SightKeeper.Domain.Model.Detector;
using SightKeeper.Infrastructure.Data;

namespace SightKeeper.Tests.DAL;

public sealed class ProfileComponentsTests : DbRelatedTests
{
	[Fact]
	public void ShouldDeleteClassesGroupOnComponentDelete()
	{
		Profile profile = new("Test profile");
		DetectorModel model = new("Test model");
		ProfileComponent component = new(profile, model);
		ItemClassGroup group = new(component);
		component.ItemClassesGroups.Add(group);
		profile.Components.Add(component);
		
		using AppDbContext dbContext = DbProvider.NewContext;
		dbContext.Profiles.Add(profile);
		dbContext.SaveChanges();

		dbContext.ProfileComponents.Should().Contain(component);
		dbContext.ItemClassesGroups.Should().Contain(group);

		dbContext.Profiles.Remove(profile);
		dbContext.SaveChanges();

		dbContext.ProfileComponents.Should().BeEmpty();
		dbContext.ItemClassesGroups.Should().BeEmpty();
	}
}