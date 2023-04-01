using System.Collections.ObjectModel;
using ReactiveUI;
using SightKeeper.Domain.Model.Abstract;

namespace SightKeeper.Domain.Model.Common;

public class ItemClassGroup : ReactiveObject, Entity
{
	public ItemClassGroup()
	{
		ItemClasses = new ObservableCollection<ItemClass>();
	}

	public int Id { get; private set; } = 0;
	public virtual ObservableCollection<ItemClass> ItemClasses { get; private set; }
}