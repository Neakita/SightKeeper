using System.Collections.ObjectModel;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using SightKeeper.Domain.Model.Abstract;

namespace SightKeeper.Domain.Model.Common;

public class Game : ReactiveObject, Entity
{
	public Game(string title, string processName)
	{
		Title = title;
		ProcessName = processName;
	}
	
	private Game(int id, string title, string processName)
	{
		Id = id;
		Title = title;
		ProcessName = processName;
	}

	public int Id { get; set; } = -1;
	
	[Reactive] public string Title { get; set; }
	
	public string ProcessName { get; private set; }

	public virtual ObservableCollection<Abstract.Model> Models { get; } = new();

	public override string ToString() => Title;
}