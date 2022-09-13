using ReactiveUI;
using SightKeeper.Backend.Data;
using SightKeeper.Backend.Data.Members.Abstract;

namespace SightKeeper.UI.WPF.ViewModels.Data;

public abstract class ModelVM : ReactiveObject
{
	public Model Model { get; }

	public string Name
	{
		get => Model.Name;
		set
		{
			using AppDbContext dbContext = new();
			Model.Name = value;
			dbContext.Update(Model);
			dbContext.SaveChanges();
			this.RaisePropertyChanged();
		}
	}
	
	
	public ModelVM(Model model)
	{
		Model = model;
	}
}
