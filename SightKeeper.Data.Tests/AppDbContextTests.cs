using SightKeeper.Domain.Model.Common;
using SightKeeper.Domain.Model.Detector;
using SightKeeper.Tests.Common;

namespace SightKeeper.Data.Tests;

public sealed class AppDbContextTests : DbRelatedTests
{
	[Fact]
	public void ShouldRollbackEntity()
	{
		const string originTestProfileName = "Test profile";
		const string changedTestProfileName = "Test profile changed name";
		
		Profile profile = new(originTestProfileName, new DetectorModel("Detector"));
		using AppDbContext dbContext = DbContextFactory.CreateDbContext();
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
		
		Profile profile = new(originTestProfileName, new DetectorModel("Detector"));
		using (AppDbContext dbContext = DbContextFactory.CreateDbContext())
		{
			dbContext.Profiles.Add(profile);
			dbContext.SaveChanges();
			dbContext.Profiles.Should().Contain(profile);
		}
		
		profile.Name = changedTestProfileName;

		profile.Name.Should().Be(changedTestProfileName);

		using (AppDbContext dbContext = DbContextFactory.CreateDbContext())
		{
			dbContext.RollBack(profile);
		}
		
		profile.Name.Should().Be(originTestProfileName);
	}

	[Fact]
	public void ShouldRollbackEntityCollection()
	{
		DetectorModel model = new("Test model");
		ItemClass firstItemClass = new("First");
		model.ItemClasses.Add(firstItemClass);
		using (AppDbContext dbContext = DbContextFactory.CreateDbContext())
		{
			dbContext.DetectorModels.Add(model);
			dbContext.SaveChanges();
			dbContext.DetectorModels.Should().Contain(model);
			dbContext.ItemClasses.Should().Contain(firstItemClass);
		}

		using (AppDbContext dbContext = DbContextFactory.CreateDbContext())
		{
			dbContext.Attach(model);
			ItemClass secondItemClass = new("Second");
			model.ItemClasses.Add(secondItemClass);
			dbContext.RollBack(model);
			model.ItemClasses.Should().OnlyContain(item => item == firstItemClass);
		}
	}
}
