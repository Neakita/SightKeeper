using NSubstitute;
using SightKeeper.Data.DataSets.Weights;
using SightKeeper.Data.Services;
using SightKeeper.Domain.DataSets.Weights;

namespace SightKeeper.Data.Tests;

public sealed class DataRemovingWeightsLibraryTests
{
	[Fact]
	public void ShouldDeleteWeightsDataWhenRemovingWeights()
	{
		var innerLibrary = Substitute.For<WeightsLibrary>();
		var library = new DataRemovingWeightsLibrary(innerLibrary);
		var weights = Substitute.For<WeightsData, DeletableData>();
		library.RemoveWeights(weights);
		((DeletableData)weights).Received().DeleteData();
	}
}