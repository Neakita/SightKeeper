using CommunityToolkit.Mvvm.ComponentModel;
using FlakeId;

namespace SightKeeper.Domain.Model;

public sealed class Game(string title, string processName) : ObservableObject
{
	public Id Id { get; private set; }

	public string Title
	{
		get => title;
		set => SetProperty(ref title, value);
	}

	public string ProcessName
	{
		get => processName;
		set => SetProperty(ref processName, value);
	}

	public override string ToString() => Title;
}