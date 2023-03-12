﻿using SightKeeper.Domain.Model.Common;
using SightKeeper.Domain.Model.Common.Synergies;
using SightKeeper.Domain.Model.Detector;
using SightKeeper.Tests.Common;

namespace SightKeeper.Infrastructure.Data.Tests;

public sealed class SynergiesTests : DbRelatedTests
{
	[Fact]
	public void ShouldDeleteSynergyOnProfileDelete()
	{
		Profile profile = new("Test profile");
		DetectorModel model = new("Test model");
		ProfileComponent component = new(profile, model);
		SingleKeySwitchSynergy synergy = new(component);
		profile.Components.Add(component);
		profile.Synergy = synergy;
		using AppDbContext dbContext = DbContextFactory.CreateDbContext();
		dbContext.Profiles.Add(profile);
		dbContext.SaveChanges();

		dbContext.Profiles.Should().Contain(profile);
		dbContext.Models.Should().Contain(model);
		dbContext.ProfileComponents.Should().Contain(component);
		dbContext.Synergies.Should().Contain(synergy);

		dbContext.Profiles.Remove(profile);
		dbContext.SaveChanges();

		dbContext.Synergies.Should().BeEmpty();
	}
}