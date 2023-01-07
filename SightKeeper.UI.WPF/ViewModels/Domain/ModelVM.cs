using System.Collections.Generic;
using ReactiveUI;
using SightKeeper.Backend.Windows;
using SightKeeper.DAL;
using SightKeeper.DAL.Domain.Abstract;
using SightKeeper.DAL.Domain.Common;
using Image = System.Drawing.Image;

namespace SightKeeper.UI.WPF.ViewModels.Domain;

public abstract class ModelVM : ReactiveObject, IModelVM
{
	protected ModelVM(Model model)
	{
		Model = model;
	}
	
	public string Name
	{
		get => Model.Name;
		set
		{
			Model.Name = value;
			this.RaisePropertyChanged();
		}
	}

	public string Description
	{
		get => Model.Description;
		set
		{
			Model.Description = value;
			this.RaisePropertyChanged();
		}
	}

	public ModelState ModelState
	{
		get => Model.State;
		set
		{
			Model.State = value;
			this.RaisePropertyChanged();
		}
	}

	public Image? Image => Model.Image == null ? null : ImageConverter.BytesToImage(Model.Image.Content);

	public Resolution Resolution { get; private set; }
	public IEnumerable<ItemClass> Classes { get; private set; }
	public Game? Game { get; set; }
	public ModelState State { get; set; }
	
	
	public Model Model { get; }
}