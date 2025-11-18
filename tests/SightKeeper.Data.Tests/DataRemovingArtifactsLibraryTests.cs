using NSubstitute;
using SightKeeper.Data.DataSets.Artifacts;
using SightKeeper.Data.Services;
using SightKeeper.Domain.DataSets.Artifacts;

namespace SightKeeper.Data.Tests;

public sealed class DataRemovingArtifactsLibraryTests
{
	[Fact]
	public void ShouldDeleteArtifactDataWhenRemovingArtifact()
	{
		var innerLibrary = Substitute.For<ArtifactsLibrary>();
		var library = new DataRemovingArtifactsLibrary(innerLibrary);
		var artifact = Substitute.For<Artifact, DeletableData>();
		library.RemoveArtifact(artifact);
		((DeletableData)artifact).Received().DeleteData();
	}
}