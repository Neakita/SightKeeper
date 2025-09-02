using System.Collections.ObjectModel;
using FluentAssertions;
using NSubstitute;
using SightKeeper.Data.DataSets.Tags;
using SightKeeper.Data.DataSets.Weights;
using SightKeeper.Domain.DataSets.Weights;
using Vibrance.Changes;

namespace SightKeeper.Data.Tests.DataSets;

public sealed class ObservableWeightsLibraryTests
{
	[Fact]
	public void ShouldObserveWeightsCreation()
	{
		var set = new ObservableWeightsLibrary(Substitute.For<StorableWeightsLibrary>());
		var observableList = (Vibrance.ReadOnlyObservableCollection<StorableWeights>)set.Weights;
		var observedChanges = new List<Change<StorableWeights>>();
		using var subscription = observableList.Subscribe(observedChanges.Add);
		var weights = set.CreateWeights(new WeightsMetadata(), ReadOnlyCollection<StorableTag>.Empty);
		var insertion = observedChanges.Should().ContainSingle().Which.Should().BeOfType<Addition<StorableWeights>>().Subject;
		insertion.Items.Should().ContainSingle().Which.Should().Be(weights);
	}

	[Fact]
	public void ShouldObserveWeightsRemoval()
	{
		var weights = Substitute.For<StorableWeights>();
		var innerSet = Substitute.For<StorableWeightsLibrary>();
		var set = new ObservableWeightsLibrary(innerSet);
		var observableList = (Vibrance.ReadOnlyObservableCollection<StorableWeights>)set.Weights;
		var observedChanges = new List<Change<Weights>>();
		using var subscription = observableList.Subscribe(observedChanges.Add);
		set.RemoveWeights(weights);
		var removal = observedChanges.Should().ContainSingle(change => change is Removal<StorableWeights>).Subject.As<Removal<StorableWeights>>();
		removal.Items.Should().ContainSingle().Which.Should().Be(weights);
	}
}