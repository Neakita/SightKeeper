using SightKeeper.Application;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.DataSets;

namespace SightKeeper.Data.Binary;

public sealed class AppData : ApplicationSettingsProvider
{
	public HashSet<DataSet> DataSets { get; }
	public HashSet<Game> Games { get; }
	public ApplicationSettings ApplicationSettings { get; }

	public bool CustomDecorations
	{
		get => ApplicationSettings.CustomDecorations;
		set => ApplicationSettings.CustomDecorations = value;
	}

	internal AppData(
		HashSet<Game> games,
		HashSet<DataSet> dataSets,
		ApplicationSettings applicationSettings)
	{
		DataSets = dataSets;
		Games = games;
		ApplicationSettings = applicationSettings;
	}

	internal AppData()
	{
		DataSets = new HashSet<DataSet>();
		Games = new HashSet<Game>();
		ApplicationSettings = new ApplicationSettings();
	}
}