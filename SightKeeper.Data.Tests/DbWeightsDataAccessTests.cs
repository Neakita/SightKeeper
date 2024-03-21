using SightKeeper.Data.Services;
using SightKeeper.Domain.Model.DataSets;
using SightKeeper.Tests.Common;

namespace SightKeeper.Data.Tests;

public sealed class DbWeightsDataAccessTests : DbRelatedTests
{
	[Fact]
	public void ShouldSaveAndLoadWeights()
	{
		var dataSet = DomainTestsHelper.NewDataSet;
		var dbContext = DbContextFactory.CreateDbContext();
		dbContext.Add(dataSet);
		DbWeightsDataAccess weightsDataAccess = new(dbContext);
		var weights = weightsDataAccess.CreateWeights(dataSet.Weights, [0], [1], ModelSize.Large, new WeightsMetrics(), Array.Empty<ItemClass>());
		dbContext.SaveChanges();
		weightsDataAccess.LoadWeightsONNXData(weights).Content.Single().Should().Be(0);
		weightsDataAccess.LoadWeightsPTData(weights).Content.Single().Should().Be(1);
	}
	
	[Fact]
	public void ShouldSaveAndLoadWeightsWithoutSaving()
	{
		var dataSet = DomainTestsHelper.NewDataSet;
		var dbContext = DbContextFactory.CreateDbContext();
		dbContext.Add(dataSet);
		DbWeightsDataAccess weightsDataAccess = new(dbContext);
		var weights = weightsDataAccess.CreateWeights(dataSet.Weights, [0], [1], ModelSize.Large, new WeightsMetrics(), Array.Empty<ItemClass>());
		weightsDataAccess.LoadWeightsONNXData(weights).Content.Single().Should().Be(0);
		weightsDataAccess.LoadWeightsPTData(weights).Content.Single().Should().Be(1);
	}
}