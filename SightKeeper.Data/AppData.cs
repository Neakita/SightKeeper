using CommunityToolkit.Diagnostics;
using SightKeeper.Application;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data;

public sealed class AppData : ApplicationSettingsProvider
{
	public IReadOnlySet<ImageSet> ScreenshotsLibraries => _screenshotsLibraries;
	public IReadOnlySet<DataSet> DataSets => _dataSets;
	public ApplicationSettings ApplicationSettings { get; }

	public bool CustomDecorations
	{
		get => ApplicationSettings.CustomDecorations;
		set => ApplicationSettings.CustomDecorations = value;
	}

	internal AppData(
		HashSet<ImageSet> screenshotsLibraries,
		HashSet<DataSet> dataSets,
		ApplicationSettings applicationSettings)
	{
		_screenshotsLibraries = screenshotsLibraries;
		_dataSets = dataSets;
		ApplicationSettings = applicationSettings;
	}

	internal AppData()
	{
		_screenshotsLibraries = new HashSet<ImageSet>();
		_dataSets = new HashSet<DataSet>();
		ApplicationSettings = new ApplicationSettings();
	}

	internal void AddScreenshotsLibrary(ImageSet library)
	{
		bool isAdded = _screenshotsLibraries.Add(library);
		Guard.IsTrue(isAdded);
	}

	internal void RemoveScreenshotsLibrary(ImageSet library)
	{
		bool isRemoved = _screenshotsLibraries.Remove(library);
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

	private readonly HashSet<ImageSet> _screenshotsLibraries;
	private readonly HashSet<DataSet> _dataSets;
}