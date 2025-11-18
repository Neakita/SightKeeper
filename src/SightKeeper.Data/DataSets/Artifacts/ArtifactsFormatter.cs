using System.Buffers;
using CommunityToolkit.Diagnostics;
using FlakeId;
using MemoryPack;
using SightKeeper.Data.ImageSets.Images;
using SightKeeper.Domain;
using SightKeeper.Domain.DataSets.Artifacts;

namespace SightKeeper.Data.DataSets.Artifacts;

internal static class ArtifactsFormatter
{
	public static void WriteArtifacts<TBufferWriter>(
		ref MemoryPackWriter<TBufferWriter> writer,
		IReadOnlyCollection<Artifact> artifacts)
		where TBufferWriter : IBufferWriter<byte>
	{
		writer.WriteCollectionHeader(artifacts.Count);
		foreach (var artifact in artifacts)
			WriteArtifact(ref writer, artifact);
	}

	public static void ReadArtifact(ref MemoryPackReader reader, ArtifactsLibrary library)
	{
		Guard.IsTrue(reader.TryReadCollectionHeader(out var artifactsCount));
		var innermostLibrary = library.GetFirst<InMemoryArtifactsLibrary>();
		for (int i = 0; i < artifactsCount; i++)
		{
			var artifact = ReadArtifacts(ref reader);
			innermostLibrary.AddArtifact(artifact);
		}
	}

	private static void WriteArtifact<TBufferWriter>(ref MemoryPackWriter<TBufferWriter> writer, Artifact artifact)
		where TBufferWriter : IBufferWriter<byte>
	{
		var artifactId = GetArtifactId(artifact);
		writer.WriteUnmanaged(artifactId);
		WriteMetadata(ref writer, artifact.Metadata);
	}

	private static InMemoryArtifact ReadArtifacts(ref MemoryPackReader reader)
	{
		var id = reader.ReadUnmanaged<Id>();
		var artifactMetadata = ReadMetadata(ref reader);
		return new InMemoryArtifact(id, artifactMetadata);
	}

	private static void WriteMetadata<TBufferWriter>(
		ref MemoryPackWriter<TBufferWriter> writer,
		ArtifactMetadata metadata)
		where TBufferWriter : IBufferWriter<byte>
	{
		writer.WriteUnmanaged(
			metadata.CreationTimestamp,
			metadata.Resolution);
		writer.WriteString(metadata.Model);
		writer.WriteString(metadata.Format);
	}

	private static ArtifactMetadata ReadMetadata(ref MemoryPackReader reader)
	{
		reader.ReadUnmanaged(
			out DateTimeOffset creationTimestamp,
			out Vector2<ushort> resolution);
		var model = reader.ReadNotNullString();
		var format = reader.ReadNotNullString();
		return new ArtifactMetadata
		{
			Model = model,
			Format = format,
			CreationTimestamp = creationTimestamp,
			Resolution = resolution
		};
	}

	private static Id GetArtifactId(Artifact artifact)
	{
		var idHolder = artifact.GetFirst<IdHolder>();
		return idHolder.Id;
	}
}