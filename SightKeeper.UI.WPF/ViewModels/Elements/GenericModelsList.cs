using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Threading;
using System.Threading.Tasks;
using ReactiveUI;
using SightKeeper.Backend.Models;
using SightKeeper.DAL.Domain.Abstract;
using SightKeeper.DAL.Domain.Common;
using SightKeeper.UI.WPF.Dialogs;
using SightKeeper.UI.WPF.Misc;
using SightKeeper.UI.WPF.ViewModels.Domain;
using SightKeeper.UI.WPF.Views.Windows;

namespace SightKeeper.UI.WPF.ViewModels.Elements;

public sealed class GenericModelsList<TModel, TModelVM> : ReactiveObject, IModelsList<TModelVM> where TModel : Model where TModelVM : ModelVM, IModelVM<TModel>
{
	public GenericModelsList(
		IModelsListInfo<TModelVM> modelsListInfo,
		IModelsProvider<TModel> modelsProvider,
		IModelsService<TModel> modelsService,
		IModelToVMStrategy<TModelVM, TModel> modelToVMStrategy,
		IDialogHost<MainWindow> dialogHost)
	{
		Label = modelsListInfo.Label;
		_modelsProvider = modelsProvider;
		_modelsService = modelsService;
		_modelToVMStrategy = modelToVMStrategy;
		_dialogHost = dialogHost;

		CreateNewModelCommand = ReactiveCommand.Create(CreateNewModel);
		DeleteModelCommand = ReactiveCommand.Create<ModelVM>(DeleteModel);
	}


	public string Label { get; }
	public IEnumerable<IModelVM> Models => _cachedModels ??= GetModelsFromProvider().ToList();


	public ReactiveCommand<Unit, Unit> CreateNewModelCommand { get; }
	public ReactiveCommand<ModelVM, Unit> DeleteModelCommand { get; }


	private readonly IModelsProvider<TModel> _modelsProvider;
	private readonly IModelsService<TModel> _modelsService;
	private readonly IModelToVMStrategy<TModelVM, TModel> _modelToVMStrategy;
	private readonly IDialogHost<MainWindow> _dialogHost;


	private void CreateNewModel()
	{
		_modelsService.Create("New model", new Resolution());
		UpdateModels();
	}

	private async Task CreateNewModelAsync(CancellationToken cancellationToken = default)
	{
		
	}

	private void DeleteModel(ModelVM model)
	{
		_modelsService.Delete((TModel) model.Model);
		UpdateModels();
	}

	private void UpdateModels()
	{
		_cachedModels = null;
		this.RaisePropertyChanged(nameof(Models));
	}

	private IEnumerable<IModelVM> GetModelsFromProvider() =>
		_modelsProvider.Models.Select(model => _modelToVMStrategy.CreateVM(model));


	private List<IModelVM>? _cachedModels;
}