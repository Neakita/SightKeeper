using CommunityToolkit.Diagnostics;
using MemoryPack;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data;

[MemoryPackable]
internal sealed partial class AppData
{
	public ushort SchemaVersion { get; }
	public HashSet<ImageSet> ImageSets { get; }
	public HashSet<DataSet<Asset>> DataSets { get; }

	[MemoryPackConstructor]
	internal AppData(
		ushort schemaVersion,
		HashSet<ImageSet> imageSets,
		HashSet<DataSet<Asset>> dataSets)
	{
		SchemaVersion = schemaVersion;
		ImageSets = imageSets;
		DataSets = dataSets;
	}

	internal AppData()
	{
		ImageSets = new HashSet<ImageSet>();
		DataSets = new HashSet<DataSet<Asset>>();
	}

	public void AddImageSet(ImageSet library)
	{
		bool isAdded = ImageSets.Add(library);
		Guard.IsTrue(isAdded);
	}

	public void RemoveImageSet(ImageSet library)
	{
		bool isRemoved = ImageSets.Remove(library);
		Guard.IsTrue(isRemoved);
	}

	public void AddDataSet(DataSet<Asset> dataSet)
	{
		bool isAdded = DataSets.Add(dataSet);
		Guard.IsTrue(isAdded);
	}

	public void RemoveDataSet(DataSet<Asset> dataSet)
	{
		bool isRemoved = DataSets.Remove(dataSet);
		Guard.IsTrue(isRemoved);
	}
}