using CommunityToolkit.Diagnostics;
using SightKeeper.Application;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.DataSets;
using SightKeeper.Domain.Model.Profiles;

namespace SightKeeper.Data.Binary;

public sealed class AppData : ApplicationSettingsProvider
{
	public IReadOnlySet<Game> Games => _games;
	public IReadOnlySet<DataSet> DataSets => _dataSets;
	public IReadOnlySet<Profile> Profiles => _profiles;
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
		_games = games;
		_dataSets = dataSets;
		_profiles = profiles;
		ApplicationSettings = applicationSettings;
	}

	internal AppData()
	{
		_games = new HashSet<Game>();
		_dataSets = new HashSet<DataSet>();
		_profiles = new HashSet<Profile>();
		ApplicationSettings = new ApplicationSettings();
	}

	internal void AddGame(Game game)
	{
		bool isAdded = _games.Add(game);
		Guard.IsTrue(isAdded);
	}

	internal void RemoveGame(Game game)
	{
		bool isRemoved = _games.Remove(game);
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

	internal void AddProfile(Profile profile)
	{
		bool isAdded = _profiles.Add(profile);
		Guard.IsTrue(isAdded);
	}

	internal void RemoveProfile(Profile profile)
	{
		bool isRemoved = _profiles.Remove(profile);
		Guard.IsTrue(isRemoved);
	}

	private readonly HashSet<Game> _games;
	private readonly HashSet<DataSet> _dataSets;
	private readonly HashSet<Profile> _profiles;
}