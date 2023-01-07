using System.Collections.Generic;
using System.Windows.Input;
using ReactiveUI;
using SightKeeper.Backend;

namespace SightKeeper.UI.WPF.ViewModels.Dialogs;

public sealed class ModelCreatorVM : ReactiveObject, IModelCreatorVM
{
	public string NewModelName { get; set; } = "New model";
	public ushort ResolutionWidth { get; set; } = 320;
	public ushort ResolutionHeight { get; set; } = 320;
	public ModelType ModelType { get; set; } = ModelType.Detector;

	public IEnumerable<ModelType> AvailableModelTypes { get; } = new[] {ModelType.Detector, ModelType.Classifier};

	public ICommand ApplyCommand { get; }
	public ICommand CancelCommand { get; }


	public ModelCreatorVM()
	{
		ApplyCommand = ReactiveCommand.Create(Apply);
		CancelCommand = ReactiveCommand.Create(Cancel);
	}
	

	private void Apply()
	{
		
	}

	private void Cancel()
	{
		
	}
}
