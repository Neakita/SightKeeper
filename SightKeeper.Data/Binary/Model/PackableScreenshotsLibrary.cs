using System.Collections.Immutable;
using FlakeId;
using MemoryPack;
using SightKeeper.Domain.Model.DataSets.Screenshots;

namespace SightKeeper.Data.Binary.Model.Screenshots;

/// <summary>
/// MemoryPackable version of <see cref="ScreenshotsLibrary"/>
/// </summary>
[MemoryPackable]
public partial class PackableScreenshotsLibrary
{
	public string Name { get; }
	public ImmutableArray<Id> Screenshots { get; }

	public PackableScreenshotsLibrary(string name, ImmutableArray<Id> screenshots)
	{
		Name = name;
		Screenshots = screenshots;
	}
}