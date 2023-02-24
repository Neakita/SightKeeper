using FluentAssertions;
using SightKeeper.Domain.Model.Common;
using SightKeeper.Domain.Model.Common.Modifiers;
using SightKeeper.Domain.Model.Detector;
using SightKeeper.Infrastructure.Data;
using SightKeeper.Tests.Common;

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
		using AppDbContext dbContext = DbContextFactory.CreateDbContext();
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
