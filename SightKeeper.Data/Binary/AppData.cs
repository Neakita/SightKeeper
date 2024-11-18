using SightKeeper.Application;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.DataSets;
using SightKeeper.Domain.Model.Profiles;

namespace SightKeeper.Data.Binary;

public sealed class AppData : ApplicationSettingsProvider
{
	public List<Game> Games { get; }
	public List<DataSet> DataSets { get; }
	public List<Profile> Profiles { get; }
	public ApplicationSettings ApplicationSettings { get; }

	public bool CustomDecorations
	{
		get => ApplicationSettings.CustomDecorations;
		set => ApplicationSettings.CustomDecorations = value;
	}

	internal AppData(
		List<Game> games,
		List<DataSet> dataSets,
		List<Profile> profiles,
		ApplicationSettings applicationSettings)
	{
		Games = games;
		DataSets = dataSets;
		Profiles = profiles;
		ApplicationSettings = applicationSettings;
	}

	internal AppData()
	{
		Games = new List<Game>();
		DataSets = new List<DataSet>();
		Profiles = new List<Profile>();
		ApplicationSettings = new ApplicationSettings();
	}
}