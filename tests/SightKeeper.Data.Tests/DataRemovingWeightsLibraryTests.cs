using NSubstitute;
using SightKeeper.Data.DataSets.Weights;

namespace SightKeeper.Data.Tests;

public sealed class DataRemovingWeightsLibraryTests
{
	[Fact]
	public void ShouldDeleteWeightsDataWhenRemovingWeights()
	{
		var innerLibrary = Substitute.For<StorableWeightsLibrary>();
		var library = new DataRemovingWeightsLibrary(innerLibrary);
		var weights = Substitute.For<StorableWeights>();
		library.RemoveWeights(weights);
		weights.Received().DeleteData();
	}
}