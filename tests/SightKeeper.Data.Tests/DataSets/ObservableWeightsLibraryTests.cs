using FluentAssertions;
using NSubstitute;
using SightKeeper.Data.DataSets.Weights;
using SightKeeper.Domain.DataSets.Weights;
using Vibrance;
using Vibrance.Changes;

namespace SightKeeper.Data.Tests.DataSets;

public sealed class ObservableWeightsLibraryTests
{
	[Fact]
	public void ShouldObserveWeightsCreation()
	{
		var set = new ObservableWeightsLibrary(Substitute.For<WeightsLibrary>());
		var observableList = (ReadOnlyObservableCollection<WeightsData>)set.Weights;
		var observedChanges = new List<Change<WeightsData>>();
		using var subscription = observableList.Subscribe(observedChanges.Add);
		var weightsMetadata = new WeightsMetadata
		{
			Model = string.Empty,
			CreationTimestamp = default,
			Resolution = default
		};
		var weights = set.CreateWeights(weightsMetadata);
		var insertion = observedChanges.Should().ContainSingle().Which.Should().BeOfType<Addition<WeightsData>>().Subject;
		insertion.Items.Should().ContainSingle().Which.Should().Be(weights);
	}

	[Fact]
	public void ShouldObserveWeightsRemoval()
	{
		var weights = Substitute.For<WeightsData>();
		var innerSet = Substitute.For<WeightsLibrary>();
		var set = new ObservableWeightsLibrary(innerSet);
		var observableList = (ReadOnlyObservableCollection<WeightsData>)set.Weights;
		var observedChanges = new List<Change<WeightsData>>();
		using var subscription = observableList.Subscribe(observedChanges.Add);
		set.RemoveWeights(weights);
		var removal = observedChanges.Should().ContainSingle(change => change is Removal<WeightsData>).Subject.As<Removal<WeightsData>>();
		removal.Items.Should().ContainSingle().Which.Should().Be(weights);
	}
}