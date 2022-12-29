using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using ReactiveUI;
using SightKeeper.Backend.Models;
using SightKeeper.DAL.Domain.Abstract;
using SightKeeper.UI.WPF.Misc;
using SightKeeper.UI.WPF.ViewModels.Domain;

namespace SightKeeper.UI.WPF.ViewModels.Elements;

public class ModelsListVM<TModelVM, TModelEntity> : ReactiveObject, IModelsListVM<TModelVM, TModelEntity>
	where TModelVM : class, IModelVM<TModelEntity> where TModelEntity : Model
{
	private readonly IModelToVMStrategy<TModelVM, TModelEntity> _entityToVMStrategy;


	private readonly IModelsProvider<TModelEntity> _modelsProvider;
	private readonly IModelsService<TModelEntity> _modelsService;


	internal ModelsListVM(IModelsProvider<TModelEntity> modelsProvider, IModelsService<TModelEntity> modelsService,
		IModelToVMStrategy<TModelVM, TModelEntity> entityToVMStrategy)
	{
		_modelsProvider = modelsProvider;
		_modelsService = modelsService;
		_entityToVMStrategy = entityToVMStrategy;
		CreateNewModelCommand = ReactiveCommand.Create(CreateNewModel);
		DeleteModelCommand = ReactiveCommand.Create<TModelVM>(DeleteModel);
	}

	public IEnumerable<TModelVM> Models => _modelsProvider.Models.Select(_entityToVMStrategy.ConvertToVM);
	public ReactiveCommand<Unit, Unit> CreateNewModelCommand { get; }
	public ReactiveCommand<TModelVM, Unit> DeleteModelCommand { get; }

	private void CreateNewModel()
	{
	}

	private void DeleteModel(TModelVM model)
	{
		_modelsService.Delete(model.Model);
		this.RaisePropertyChanged(nameof(Models));
	}
}