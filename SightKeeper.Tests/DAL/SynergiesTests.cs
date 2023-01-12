﻿using FluentAssertions;
using SightKeeper.Domain.Common;
using SightKeeper.Domain.Common.Synergies;
using SightKeeper.Domain.Detector;
using SightKeeper.Persistance;

namespace SightKeeper.Tests.DAL;

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
		using AppDbContext dbContext = DbProvider.NewContext;
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
