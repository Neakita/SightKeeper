using System.Collections.Generic;
using System.Windows.Input;
using ReactiveUI;
using SightKeeper.Application;

namespace SightKeeper.UI.WPF.ViewModels.Dialogs;

public sealed class ModelCreatorVM : ReactiveObject, IModelCreatorVM
{
	public string Name { get; set; } = "New model";
	public ushort ResolutionWidth { get; set; } = 320;
	public ushort ResolutionHeight { get; set; } = 320;
	public ModelType Type { get; set; } = ModelType.Detector;

	public IEnumerable<ModelType> AvailableTypes { get; } = new[] {ModelType.Detector, ModelType.Classifier};

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
