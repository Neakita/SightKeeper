using CommunityToolkit.Diagnostics;
using FlakeId;
using SightKeeper.Data.Services;
using SightKeeper.Domain.DataSets.Artifacts;

namespace SightKeeper.Data.DataSets.Artifacts;

internal sealed class InMemoryArtifactsLibrary(Wrapper<Artifact> artifactWrapper) : ArtifactsLibrary
{
	public IReadOnlyCollection<Artifact> Artifacts => _artifacts;

	public Artifact CreateArtifact(ArtifactMetadata metadata)
	{
		var id = Id.Create();
		var inMemoryArtifact = new InMemoryArtifact(id, metadata);
		var wrappedArtifact = artifactWrapper.Wrap(inMemoryArtifact);
		_artifacts.Add(wrappedArtifact);
		return wrappedArtifact;
	}

	public void RemoveArtifact(Artifact artifact)
	{
		var isRemoved = _artifacts.Remove(artifact);
		Guard.IsTrue(isRemoved);
	}

	public void AddArtifact(Artifact artifact)
	{
		var wrappedArtifact = artifactWrapper.Wrap(artifact);
		_artifacts.Add(wrappedArtifact);
	}

	private readonly List<Artifact> _artifacts = new();
}