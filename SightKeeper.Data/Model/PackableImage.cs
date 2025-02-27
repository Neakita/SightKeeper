using FlakeId;
using MemoryPack;
using SightKeeper.Domain;

namespace SightKeeper.Data.Model;

/// <summary>
/// MemoryPackable version of <see cref="Domain.Images.Image"/>
/// </summary>
[MemoryPackable]
internal sealed partial class PackableImage
{
	public required Id Id { get; init; }
	public required DateTimeOffset CreationTimestamp { get; init; }
	public required Vector2<ushort> Image { get; init; }
}