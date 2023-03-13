using ReactiveUI;
using SightKeeper.Domain.Model.Common;

namespace SightKeeper.UI.WPF.ViewModels.Domain;

public sealed class ItemClassVM : ReactiveObject
{
	public string Name
	{
		get => ItemClass.Name;
		set
		{
			ItemClass.Name = value;
			this.RaisePropertyChanged();
		}
	}
	
	
	public ItemClassVM(ItemClass itemClass)
	{
		ItemClass = itemClass;
	}
	
	
	public ItemClass ItemClass { get; }
}
