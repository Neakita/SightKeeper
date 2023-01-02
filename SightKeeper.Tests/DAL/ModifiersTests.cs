using FluentAssertions;
using SightKeeper.DAL;
using SightKeeper.DAL.Domain.Common;
using SightKeeper.DAL.Domain.Common.Modifiers;
using SightKeeper.DAL.Domain.Detector;

namespace SightKeeper.Tests.DAL;

public sealed class ModifiersTests : DbRelatedTests
{
	[Fact]
	public void ShouldDeleteModifiersOnProfileDelete()
	{
		Profile profile = new("Test profile");
		DetectorModel model = new("Test model");
		ProfileComponent component = new(profile, model);
		ResolutionMultiplierModifier modifier = new(component);
		component.Modifiers.Add(modifier);
		using AppDbContext dbContext = DbProvider.NewContext;
		dbContext.ProfileComponents.Add(component);
		dbContext.SaveChanges();

		dbContext.Profiles.Should().Contain(profile);
		dbContext.Models.Should().Contain(model);
		dbContext.ProfileComponents.Should().Contain(component);
		dbContext.Modifiers.Should().Contain(modifier);

		dbContext.Profiles.Remove(profile);
		dbContext.SaveChanges();

		dbContext.Modifiers.Should().BeEmpty();
	}
}
