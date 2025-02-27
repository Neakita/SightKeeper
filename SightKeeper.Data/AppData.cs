using CommunityToolkit.Diagnostics;
using SightKeeper.Application;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data;

public sealed class AppData : ApplicationSettingsProvider
{
	public IReadOnlySet<ImageSet> ImageSets => _imageSets;
	public IReadOnlySet<DataSet> DataSets => _dataSets;
	public ApplicationSettings ApplicationSettings { get; }

	public bool CustomDecorations
	{
		get => ApplicationSettings.CustomDecorations;
		set => ApplicationSettings.CustomDecorations = value;
	}

	internal AppData(
		HashSet<ImageSet> imageSets,
		HashSet<DataSet> dataSets,
		ApplicationSettings applicationSettings)
	{
		_imageSets = imageSets;
		_dataSets = dataSets;
		ApplicationSettings = applicationSettings;
	}

	internal AppData()
	{
		_imageSets = new HashSet<ImageSet>();
		_dataSets = new HashSet<DataSet>();
		ApplicationSettings = new ApplicationSettings();
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