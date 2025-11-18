using FluentAssertions;
using NSubstitute;
using SightKeeper.Data.DataSets.Artifacts;
using SightKeeper.Domain.DataSets.Artifacts;
using Vibrance;
using Vibrance.Changes;

namespace SightKeeper.Data.Tests.DataSets;

public sealed class ObservableArtifactsLibraryTests
{
	[Fact]
	public void ShouldObserveArtifactCreation()
	{
		var set = new ObservableArtifactsLibrary(Substitute.For<ArtifactsLibrary>());
		var observableList = (ReadOnlyObservableCollection<Artifact>)set.Artifacts;
		var observedChanges = new List<Change<Artifact>>();
		using var subscription = observableList.Subscribe(observedChanges.Add);
		var artifactMetadata = new ArtifactMetadata
		{
			Model = string.Empty,
			Format = string.Empty,
			CreationTimestamp = default,
			Resolution = default
		};
		var artifact = set.CreateArtifact(artifactMetadata);
		var insertion = observedChanges.Should().ContainSingle().Which.Should().BeOfType<Addition<Artifact>>().Subject;
		insertion.Items.Should().ContainSingle().Which.Should().Be(artifact);
	}

	[Fact]
	public void ShouldObserveArtifactRemoval()
	{
		var artifact = Substitute.For<Artifact>();
		var innerSet = Substitute.For<ArtifactsLibrary>();
		var set = new ObservableArtifactsLibrary(innerSet);
		var observableList = (ReadOnlyObservableCollection<Artifact>)set.Artifacts;
		var observedChanges = new List<Change<Artifact>>();
		using var subscription = observableList.Subscribe(observedChanges.Add);
		set.RemoveArtifact(artifact);
		var removal = observedChanges.Should().ContainSingle(change => change is Removal<Artifact>).Subject.As<Removal<Artifact>>();
		removal.Items.Should().ContainSingle().Which.Should().Be(artifact);
	}
}