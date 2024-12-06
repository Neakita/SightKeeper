using FlakeId;
using MemoryPack;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.DataSets.Screenshots;

namespace SightKeeper.Data.Binary.Model;

/// <summary>
/// MemoryPackable version of <see cref="Screenshot"/>
/// </summary>
[MemoryPackable]
internal sealed partial class PackableScreenshot
{
	public required Id Id { get; init; }
	public required DateTimeOffset CreationDate { get; init; }
	public required Vector2<ushort> ImageSize { get; init; }
}