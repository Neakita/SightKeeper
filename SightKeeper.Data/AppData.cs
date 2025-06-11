using CommunityToolkit.Diagnostics;
using MemoryPack;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data;

[MemoryPackable]
internal sealed partial class AppData
{
	public HashSet<ImageSet> ImageSets { get; }
	public HashSet<DataSet> DataSets { get; }

	[MemoryPackConstructor]
	internal AppData(
		HashSet<ImageSet> imageSets,
		HashSet<DataSet> dataSets)
	{
		ImageSets = imageSets;
		DataSets = dataSets;
	}

	internal AppData()
	{
		ImageSets = new HashSet<ImageSet>();
		DataSets = new HashSet<DataSet>();
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