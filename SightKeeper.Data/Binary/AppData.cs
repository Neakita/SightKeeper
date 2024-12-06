using CommunityToolkit.Diagnostics;
using SightKeeper.Application;
using SightKeeper.Domain.Model.DataSets;
using SightKeeper.Domain.Model.DataSets.Screenshots;

namespace SightKeeper.Data.Binary;

public sealed class AppData : ApplicationSettingsProvider
{
	public IReadOnlySet<ScreenshotsLibrary> ScreenshotsLibraries => _screenshotsLibraries;
	public IReadOnlySet<DataSet> DataSets => _dataSets;
	public ApplicationSettings ApplicationSettings { get; }

	public bool CustomDecorations
	{
		get => ApplicationSettings.CustomDecorations;
		set => ApplicationSettings.CustomDecorations = value;
	}

	internal AppData(
		HashSet<ScreenshotsLibrary> screenshotsLibraries,
		HashSet<DataSet> dataSets,
		ApplicationSettings applicationSettings)
	{
		_screenshotsLibraries = screenshotsLibraries;
		_dataSets = dataSets;
		ApplicationSettings = applicationSettings;
	}

	internal AppData()
	{
		_screenshotsLibraries = new HashSet<ScreenshotsLibrary>();
		_dataSets = new HashSet<DataSet>();
		ApplicationSettings = new ApplicationSettings();
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

	private readonly HashSet<ScreenshotsLibrary> _screenshotsLibraries;
	private readonly HashSet<DataSet> _dataSets;
}