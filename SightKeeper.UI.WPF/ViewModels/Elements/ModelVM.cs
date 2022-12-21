using ReactiveUI;
using SightKeeper.DAL.Members.Abstract;

namespace SightKeeper.UI.WPF.ViewModels.Elements;

public abstract class ModelVM : ReactiveObject
{
	public string Name
	{
		get => Model.Name;
		set
		{
			Model.Name = value;
			this.RaisePropertyChanged();
		}
	}


	protected ModelVM(Model model)
	{
		Model = model;
	}


	protected readonly Model Model;
}