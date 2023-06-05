using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Domain.Model.Abstract;

[Table("Models")]
public abstract class Model : ReactiveObject, Entity
{
	public int Id { get; private set; }
	[Reactive] public string Name { get; set; }
	[Reactive] public string Description { get; set; }
	public Resolution Resolution { get; private set; }
	public ObservableCollection<ItemClass> ItemClasses { get; private set; }
	public int? GameId { get; private set; }
	[Reactive] public Game? Game { get; set; }
	public int? ConfigId { get; private set; }
	
	
	public Model(string name) : this(name, new Resolution())
	{
	}

	public Model(string name, Resolution resolution)
	{
		Name = name;
		Description = string.Empty;
		Resolution = resolution;
		ItemClasses = new ObservableCollection<ItemClass>();
	}


	protected Model(int id, string name, string description)
	{
		Id = id;
		Name = name;
		Description = description;
		Resolution = null!;
		ItemClasses = null!;
	}
}