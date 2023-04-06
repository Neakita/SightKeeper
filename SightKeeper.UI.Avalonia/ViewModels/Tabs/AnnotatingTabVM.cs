using Avalonia.Input;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using SightKeeper.Application;
using SightKeeper.Domain.Model.Abstract;
using SightKeeper.Domain.Model.Common;
using SightKeeper.Domain.Model.Detector;
using SightKeeper.Domain.Services;
using SightKeeper.Infrastructure.Common;

namespace SightKeeper.UI.Avalonia.ViewModels.Tabs;

public sealed class AnnotatingTabVM : ViewModel
{
	public static AnnotatingTabVM New => Locator.Resolve<AnnotatingTabVM>();
	
	public Repository<Model> ModelsRepository { get; }
	public ModelScreenshoter ModelScreenshoter { get; }

	public Model? SelectedModel
	{
		get => _selectedModel;
		set
		{
			this.RaiseAndSetIfChanged(ref _selectedModel, value);
			if (value == null) return;
			ModelScreenshoter.Model = value;
			SelectedDetectorModel = value as DetectorModel;
			this.RaisePropertyChanged(nameof(SelectedDetectorModel));
		}
	}

	[Reactive] public DetectorModel? SelectedDetectorModel { get; private set; }

	[Reactive] public ItemClass? SelectedItemClass { get; set; }

	public AnnotatingTabVM(Repository<Model> modelsRepository, ModelScreenshoter modelScreenshoter)
	{
		ModelsRepository = modelsRepository;
		ModelScreenshoter = modelScreenshoter;
	}

	public void OnKeyDown(Key key)
	{
		if (SelectedModel == null) return;
		if (key is < Key.D1 or > Key.D9) return;
		int itemClassIndex = key - Key.D1;
		if (itemClassIndex < SelectedModel.ItemClasses.Count)
			SelectedItemClass = SelectedModel.ItemClasses[itemClassIndex];
	}
	
	private Model? _selectedModel;
}