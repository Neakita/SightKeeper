using MemoryPack;

namespace SightKeeper.Data.Binary.DataSets;

[MemoryPackable]
[MemoryPackUnion(0, typeof(Detector.SerializableDetectorDataSet))]
public abstract partial class SerializableDataSet
{
	public string Name { get; }
	public string Description { get; }
	public ushort? GameId { get; }
	public ushort Resolution { get; }
	public ushort? MaxScreenshots { get; }
	public IReadOnlyCollection<SerializableScreenshot> Screenshots { get; }

	protected SerializableDataSet(string name,
		string description,
		ushort? gameId,
		ushort resolution,
		ushort? maxScreenshots,
		IReadOnlyCollection<SerializableScreenshot> screenshots)
	{
		Name = name;
		Description = description;
		GameId = gameId;
		Resolution = resolution;
		MaxScreenshots = maxScreenshots;
		Screenshots = screenshots;
	}
}