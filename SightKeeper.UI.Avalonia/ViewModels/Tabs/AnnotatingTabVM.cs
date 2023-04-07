using System;
using Avalonia.Input;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using SightKeeper.Application.Annotating;
using SightKeeper.Domain.Model.Abstract;
using SightKeeper.Domain.Model.Common;
using SightKeeper.Domain.Model.Detector;
using SightKeeper.Domain.Services;
using SightKeeper.Infrastructure.Common;

namespace SightKeeper.UI.Avalonia.ViewModels.Tabs;

public sealed class AnnotatingTabVM : ViewModel
{
	public static AnnotatingTabVM New => Locator.Resolve<AnnotatingTabVM>();
	
	public Repository<DetectorModel> ModelsRepository { get; }
	public DetectorAnnotator Annotator { get; }
	public ModelScreenshoter Screenshoter { get; }

	[Reactive] public ItemClass? SelectedItemClass { get; set; }

	public DetectorModel? Model
	{
		get => _model;
		set
		{
			Screenshoter.Model = value;
			Annotator.Model = value;
			this.RaiseAndSetIfChanged(ref _model, value);
		}
	}

	public AnnotatingTabVM(Repository<DetectorModel> modelsRepository, DetectorAnnotator annotator, ModelScreenshoter screenshoter)
	{
		ModelsRepository = modelsRepository;
		Annotator = annotator;
		Screenshoter = screenshoter;
	}

	public void NotifyKeyDown(Key key)
	{
		Model? model = Annotator.Model;
		if (model == null) return;
		if (key is < Key.D1 or > Key.D9) return;
		int itemClassIndex = key - Key.D1;
		if (itemClassIndex < model.ItemClasses.Count)
			SelectedItemClass = model.ItemClasses[itemClassIndex];
	}
	
	private DetectorModel? _model;
}