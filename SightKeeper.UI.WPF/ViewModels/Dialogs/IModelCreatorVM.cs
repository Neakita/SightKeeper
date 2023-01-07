using System.Collections.Generic;
using System.Windows.Input;
using SightKeeper.Backend;

namespace SightKeeper.UI.WPF.ViewModels.Dialogs;

public interface IModelCreatorVM
{
	string NewModelName { get; set; }
	ushort ResolutionWidth { get; set; }
	ushort ResolutionHeight { get; set; }
	ModelType ModelType { get; set; }
	
	IEnumerable<ModelType> AvailableModelTypes { get; }

	ICommand ApplyCommand { get; }
	ICommand CancelCommand { get; }
}