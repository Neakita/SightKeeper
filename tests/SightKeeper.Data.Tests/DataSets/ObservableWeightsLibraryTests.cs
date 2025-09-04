using System.Collections.ObjectModel;
using FluentAssertions;
using NSubstitute;
using SightKeeper.Data.DataSets.Weights;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.DataSets.Weights;
using Vibrance.Changes;

namespace SightKeeper.Data.Tests.DataSets;

public sealed class ObservableWeightsLibraryTests
{
	[Fact]
	public void ShouldObserveWeightsCreation()
	{
		var set = new ObservableWeightsLibrary(Substitute.For<WeightsLibrary>());
		var observableList = (Vibrance.ReadOnlyObservableCollection<Weights>)set.Weights;
		var observedChanges = new List<Change<Weights>>();
		using var subscription = observableList.Subscribe(observedChanges.Add);
		var weights = set.CreateWeights(new WeightsMetadata(), ReadOnlyCollection<Tag>.Empty);
		var insertion = observedChanges.Should().ContainSingle().Which.Should().BeOfType<Addition<Weights>>().Subject;
		insertion.Items.Should().ContainSingle().Which.Should().Be(weights);
	}

	[Fact]
	public void ShouldObserveWeightsRemoval()
	{
		var weights = Substitute.For<Weights>();
		var innerSet = Substitute.For<WeightsLibrary>();
		var set = new ObservableWeightsLibrary(innerSet);
		var observableList = (Vibrance.ReadOnlyObservableCollection<Weights>)set.Weights;
		var observedChanges = new List<Change<Weights>>();
		using var subscription = observableList.Subscribe(observedChanges.Add);
		set.RemoveWeights(weights);
		var removal = observedChanges.Should().ContainSingle(change => change is Removal<Weights>).Subject.As<Removal<Weights>>();
		removal.Items.Should().ContainSingle().Which.Should().Be(weights);
	}
}