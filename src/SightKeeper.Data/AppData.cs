using CommunityToolkit.Diagnostics;
using MemoryPack;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data;

[MemoryPackable]
internal sealed partial class AppData
{
	public ushort SchemaVersion { get; }
	public HashSet<ImageSet> ImageSets { get; }
	public HashSet<DataSet<Tag, Asset>> DataSets { get; }

	[MemoryPackConstructor]
	internal AppData(
		ushort schemaVersion,
		HashSet<ImageSet> imageSets,
		HashSet<DataSet<Tag, Asset>> dataSets)
	{
		SchemaVersion = schemaVersion;
		ImageSets = imageSets;
		DataSets = dataSets;
	}

	internal AppData()
	{
		ImageSets = new HashSet<ImageSet>();
		DataSets = new HashSet<DataSet<Tag, Asset>>();
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

	public void AddDataSet(DataSet<Tag, Asset> dataSet)
	{
		bool isAdded = DataSets.Add(dataSet);
		Guard.IsTrue(isAdded);
	}

	public void RemoveDataSet(DataSet<Tag, Asset> dataSet)
	{
		bool isRemoved = DataSets.Remove(dataSet);
		Guard.IsTrue(isRemoved);
	}
}