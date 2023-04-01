using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using SightKeeper.Domain.Model.Abstract;

namespace SightKeeper.Domain.Model.Common;

public class ItemClass : ReactiveObject, Entity
{
	public ItemClass(string name)
	{
		Name = name;
	}


	private ItemClass(int id, string name)
	{
		Id = id;
		Name = name;
	}

	public int Id { get; private set; }
	[Reactive] public string Name { get; set; }

	public override string ToString() => Name;
}