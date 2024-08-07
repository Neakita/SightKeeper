using SightKeeper.Application;
using SightKeeper.Domain.Model.DataSets;
using SightKeeper.Domain.Model.Profiles;

namespace SightKeeper.Data.Binary;

public sealed class AppData : ApplicationSettingsProvider
{
	public HashSet<DataSet> DataSets { get; }
	public HashSet<Domain.Model.Game> Games { get; }
	public HashSet<Profile> Profiles { get; }
	public ApplicationSettings ApplicationSettings { get; }

	public bool CustomDecorations
	{
		get => ApplicationSettings.CustomDecorations;
		set => ApplicationSettings.CustomDecorations = value;
	}

	internal AppData(
		HashSet<Domain.Model.Game> games,
		HashSet<DataSet> dataSets,
		HashSet<Profile> profiles,
		ApplicationSettings applicationSettings)
	{
		DataSets = dataSets;
		Games = games;
		Profiles = profiles;
		ApplicationSettings = applicationSettings;
	}

	internal AppData()
	{
		DataSets = new HashSet<DataSet>();
		Games = new HashSet<Domain.Model.Game>();
		Profiles = new HashSet<Profile>();
		ApplicationSettings = new ApplicationSettings();
	}
}