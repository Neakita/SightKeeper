using CommunityToolkit.Diagnostics;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data;

public sealed class AppData
{
	public IReadOnlySet<ImageSet> ImageSets => _imageSets;
	public IReadOnlySet<DataSet> DataSets => _dataSets;

	internal AppData(
		HashSet<ImageSet> imageSets,
		HashSet<DataSet> dataSets)
	{
		_imageSets = imageSets;
		_dataSets = dataSets;
	}

	internal AppData()
	{
		_imageSets = new HashSet<ImageSet>();
		_dataSets = new HashSet<DataSet>();
	}

	internal void AddImageSet(ImageSet library)
	{
		bool isAdded = _imageSets.Add(library);
		Guard.IsTrue(isAdded);
	}

	internal void RemoveImageSet(ImageSet library)
	{
		bool isRemoved = _imageSets.Remove(library);
		Guard.IsTrue(isRemoved);
	}

	internal void AddDataSet(DataSet dataSet)
	{
		bool isAdded = _dataSets.Add(dataSet);
		Guard.IsTrue(isAdded);
	}

	internal void RemoveDataSet(DataSet dataSet)
	{
		bool isRemoved = _dataSets.Remove(dataSet);
		Guard.IsTrue(isRemoved);
	}

	private readonly HashSet<ImageSet> _imageSets;
	private readonly HashSet<DataSet> _dataSets;
}