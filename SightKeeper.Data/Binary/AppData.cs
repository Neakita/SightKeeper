using SightKeeper.Application;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.DataSets;
using SightKeeper.Domain.Model.Profiles;

namespace SightKeeper.Data.Binary;

public sealed class AppData : ApplicationSettingsProvider
{
	public HashSet<Game> Games { get; }
	public HashSet<DataSet> DataSets { get; }
	public HashSet<Profile> Profiles { get; }
	public ApplicationSettings ApplicationSettings { get; }

	public bool CustomDecorations
	{
		get => ApplicationSettings.CustomDecorations;
		set => ApplicationSettings.CustomDecorations = value;
	}

	internal AppData(
		HashSet<Game> games,
		HashSet<DataSet> dataSets,
		HashSet<Profile> profiles,
		ApplicationSettings applicationSettings)
	{
		Games = games;
		DataSets = dataSets;
		Profiles = profiles;
		ApplicationSettings = applicationSettings;
	}

	internal AppData()
	{
		Games = new HashSet<Game>();
		DataSets = new HashSet<DataSet>();
		Profiles = new HashSet<Profile>();
		ApplicationSettings = new ApplicationSettings();
	}
}