using NSubstitute;
using SightKeeper.Data.DataSets.Weights;
using SightKeeper.Domain.DataSets.Weights;

namespace SightKeeper.Data.Tests;

public sealed class DataRemovingWeightsLibraryTests
{
	[Fact]
	public void ShouldDeleteWeightsDataWhenRemovingWeights()
	{
		var innerLibrary = Substitute.For<WeightsLibrary>();
		var library = new DataRemovingWeightsLibrary(innerLibrary);
		var weights = Substitute.For<Weights>();
		library.RemoveWeights(weights);
		weights.Received().DeleteData();
	}
}