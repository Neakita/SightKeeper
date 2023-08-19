using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.Detector;
using SightKeeper.Tests.Common;

namespace SightKeeper.Data.Tests;

public sealed class ProfilesTests : DbRelatedTests
{
	[Fact]
	public void ShouldCreateProfile()
	{
		using AppDbContext dbContext = DbContextFactory.CreateDbContext();
		Profile profile = new("Test profile", new DataSet<DetectorAsset>("Detector"));

		dbContext.Profiles.Add(profile);
		dbContext.SaveChanges();

		dbContext.Profiles.Should().Contain(profile);
	}

	[Fact]
	public void ShouldNotDeleteModelOnProfileDelete()
	{
		using AppDbContext dbContext = DbContextFactory.CreateDbContext();
		DataSet<DetectorAsset> dataSet = new("Test model");
		Profile profile = new("Test profile", dataSet);
		dbContext.Profiles.Add(profile);
		dbContext.SaveChanges();

		dbContext.Profiles.Should().Contain(profile);
		dbContext.DetectorDataSets.Should().Contain(dataSet);

		dbContext.Profiles.Remove(profile);
		dbContext.SaveChanges(); 

		dbContext.Profiles.Should().BeEmpty();
		dbContext.DetectorDataSets.Should().Contain(dataSet);
	}
}