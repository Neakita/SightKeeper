using FluentAssertions;
using SightKeeper.Domain.Common;
using SightKeeper.Persistance;

namespace SightKeeper.Tests.DAL;

public sealed class AppDbContextTests : DbRelatedTests
{
	[Fact]
	public void ShouldRollbackEntity()
	{
		const string originTestProfileName = "Test profile";
		const string changedTestProfileName = "Test profile changed name";
		
		Profile profile = new(originTestProfileName);
		using AppDbContext dbContext = DbProvider.NewContext;
		dbContext.Profiles.Add(profile);
		dbContext.SaveChanges();
		dbContext.Profiles.Should().Contain(profile);
		profile.Name = changedTestProfileName;

		profile.Name.Should().Be(changedTestProfileName);
		
		dbContext.RollBack(profile);

		profile.Name.Should().Be(originTestProfileName);
	}

	[Fact]
	public void ShouldRollbackDetachedEntity()
	{
		const string originTestProfileName = "Test profile";
		const string changedTestProfileName = "Test profile changed";
		
		Profile profile = new(originTestProfileName);
		using (AppDbContext dbContext = DbProvider.NewContext)
		{
			dbContext.Profiles.Add(profile);
			dbContext.SaveChanges();
			dbContext.Profiles.Should().Contain(profile);
		}
		
		profile.Name = changedTestProfileName;

		profile.Name.Should().Be(changedTestProfileName);

		using (AppDbContext dbContext = DbProvider.NewContext)
		{
			dbContext.RollBack(profile);
		}
		
		profile.Name.Should().Be(originTestProfileName);
	}
}
