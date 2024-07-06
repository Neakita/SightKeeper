using FlakeId;
using MemoryPack;
using SightKeeper.Domain.Model.DataSets;

namespace SightKeeper.Data.Binary;

[MemoryPackable]
public partial record SerializableDetectorDataSet(
	string Name,
	string Description,
	ushort? GameId,
	ushort Resolution,
	ushort? MaxScreenshots,
	IReadOnlyCollection<SerializableTag> Tags,
	IReadOnlyCollection<Id> Screenshots,
	IReadOnlyCollection<SerializableDetectorAsset> Assets,
	IReadOnlyCollection<SerializableDetectorWeights> Weights)
	: SerializableDataSet(Name, Description, GameId, Resolution, MaxScreenshots);

[MemoryPackable]
public partial record SerializableDetectorAsset(
	Id ScreenshotId,
	AssetUsage Usage,
	IReadOnlyCollection<SerializableDetectorItem> Items)
	: SerializableAsset(ScreenshotId, Usage);

[MemoryPackable]
public partial record SerializableDetectorItem(Id TagId, Bounding Bounding);

[MemoryPackable]
public partial record SerializableDetectorWeights(
	Id Id,
	DateTime CreationDate,
	ModelSize Size,
	WeightsMetrics Metrics,
	IReadOnlyCollection<Id> Tags)
	: SerializableWeights(Id, CreationDate, Size, Metrics);