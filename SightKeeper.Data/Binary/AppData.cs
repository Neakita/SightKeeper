using CommunityToolkit.Diagnostics;
using SightKeeper.Application;
using SightKeeper.Domain.Model.DataSets;

namespace SightKeeper.Data.Binary;

public sealed class AppData : ApplicationSettingsProvider
{
	public IReadOnlySet<DataSet> DataSets => _dataSets;
	public ApplicationSettings ApplicationSettings { get; }

	public bool CustomDecorations
	{
		get => ApplicationSettings.CustomDecorations;
		set => ApplicationSettings.CustomDecorations = value;
	}

	internal AppData(
		HashSet<DataSet> dataSets,
		ApplicationSettings applicationSettings)
	{
		_dataSets = dataSets;
		ApplicationSettings = applicationSettings;
	}

	internal AppData()
	{
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

	private readonly HashSet<DataSet> _dataSets;
}