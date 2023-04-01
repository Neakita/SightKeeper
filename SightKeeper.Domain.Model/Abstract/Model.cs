using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Domain.Model.Abstract;

[Table("Models")]
public abstract class Model : ReactiveObject, Entity
{
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


	protected Model(int id, string name)
	{
		Id = id;
		Name = name;
		Description = null!;
		Resolution = null!;
		ItemClasses = null!;
	}

	public int Id { get; private set; }

	[Reactive] public string Name { get; set; }
	[Reactive] public string Description { get; set; }
	
	public Resolution Resolution { get; private set; }
	public ObservableCollection<ItemClass> ItemClasses { get; private set; }
	[Reactive] public Game? Game { get; set; }
	[Reactive] public ModelConfig? Config { get; set; }
}