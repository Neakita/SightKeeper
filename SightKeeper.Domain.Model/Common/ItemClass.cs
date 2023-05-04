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
	public int ModelId { get; private set; }
	public Abstract.Model Model { get; private set; }

	public override string ToString() => Name;
}