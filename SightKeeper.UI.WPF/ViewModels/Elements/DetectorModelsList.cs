using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using ReactiveUI;
using SightKeeper.Abstractions.Domain;
using SightKeeper.Backend.Models;
using SightKeeper.UI.WPF.ViewModels.Domain;

namespace SightKeeper.UI.WPF.ViewModels.Elements;

public sealed class DetectorModelsList : ReactiveObject, IModelsListVM<DetectorModelVM, IDetectorModel>
{
	public IEnumerable<DetectorModelVM> Models => _modelsProvider.Models.Select(model => new DetectorModelVM(model));
	public ReactiveCommand<Unit, Unit> CreateNewModelCommand { get; }
	public ReactiveCommand<DetectorModelVM, Unit> DeleteModelCommand { get; }


	private DetectorModelsList(IModelsProvider<IDetectorModel> modelsProvider,
		IModelsService<IDetectorModel> modelsService)
	{
		_modelsProvider = modelsProvider;
		_modelsService = modelsService;
		CreateNewModelCommand = ReactiveCommand.Create(CreateNewModel);
		DeleteModelCommand = ReactiveCommand.Create<DetectorModelVM>(DeleteModel);
	}


	private readonly IModelsProvider<IDetectorModel> _modelsProvider;
	private readonly IModelsService<IDetectorModel> _modelsService;


	private void CreateNewModel()
	{
		
	}

	private void DeleteModel(DetectorModelVM model)
	{
		_modelsService.Delete(model.Model);
		this.RaisePropertyChanged(nameof(Models));
	}
}
