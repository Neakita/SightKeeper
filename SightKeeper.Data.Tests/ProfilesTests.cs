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
	public void ShouldNotDeleteDataSetOnProfileDelete()
	{
		using var dbContext = DbContextFactory.CreateDbContext();
		var dataSet = DomainTestsHelper.NewDetectorDataSet;
		Profile profile = new("Test profile", dataSet);
		dbContext.Profiles.Add(profile);
		dbContext.SaveChanges();

		dbContext.Profiles.Should().Contain(profile);
		dbContext.DataSets.Should().Contain(dataSet);

		dbContext.Profiles.Remove(profile);
		dbContext.SaveChanges(); 

		dbContext.Profiles.Should().BeEmpty();
		dbContext.DataSets.Should().Contain(dataSet);
	}
}