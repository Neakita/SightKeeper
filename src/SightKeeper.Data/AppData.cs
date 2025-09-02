using CommunityToolkit.Diagnostics;
using MemoryPack;
using SightKeeper.Data.ImageSets;
using SightKeeper.Domain.DataSets;

namespace SightKeeper.Data;

[MemoryPackable]
internal sealed partial class AppData
{
	public ushort SchemaVersion { get; }
	public HashSet<StorableImageSet> ImageSets { get; }
	public HashSet<DataSet> DataSets { get; }

	[MemoryPackConstructor]
	internal AppData(
		ushort schemaVersion,
		HashSet<StorableImageSet> imageSets,
		HashSet<DataSet> dataSets)
	{
		SchemaVersion = schemaVersion;
		ImageSets = imageSets;
		DataSets = dataSets;
	}

	internal AppData()
	{
		ImageSets = new HashSet<StorableImageSet>();
		DataSets = new HashSet<DataSet>();
	}

	public void AddImageSet(StorableImageSet library)
	{
		bool isAdded = ImageSets.Add(library);
		Guard.IsTrue(isAdded);
	}

	public void RemoveImageSet(StorableImageSet library)
	{
		bool isRemoved = ImageSets.Remove(library);
		Guard.IsTrue(isRemoved);
	}

	public void AddDataSet(DataSet dataSet)
	{
		bool isAdded = DataSets.Add(dataSet);
		Guard.IsTrue(isAdded);
	}

	public void RemoveDataSet(DataSet dataSet)
	{
		bool isRemoved = DataSets.Remove(dataSet);
		Guard.IsTrue(isRemoved);
	}
}