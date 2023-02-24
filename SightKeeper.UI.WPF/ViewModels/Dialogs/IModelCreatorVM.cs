using System.Collections.Generic;
using System.Windows.Input;
using SightKeeper.Application;

namespace SightKeeper.UI.WPF.ViewModels.Dialogs;

public interface IModelCreatorVM
{
	string Name { get; set; }
	ushort ResolutionWidth { get; set; }
	ushort ResolutionHeight { get; set; }
	ModelType Type { get; set; }
	
	IEnumerable<ModelType> AvailableTypes { get; }

	ICommand ApplyCommand { get; }
	ICommand CancelCommand { get; }
}