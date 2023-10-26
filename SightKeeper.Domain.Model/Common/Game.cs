using CommunityToolkit.Mvvm.ComponentModel;
using FlakeId;

namespace SightKeeper.Domain.Model.Common;

public sealed class Game : ObservableObject
{
	public Id Id { get; private set; }

	public string Title
	{
		get => _title;
		set => SetProperty(ref _title, value);
	}

	public string ProcessName
	{
		get => _processName;
		set => SetProperty(ref _processName, value);
	}

	public Game(string title, string processName)
	{
		_title = title;
		_processName = processName;
	}

	public override string ToString() => Title;
	
	private string _title;
	private string _processName;
}