using System.Collections.Generic;
using System.Reactive;
using ReactiveUI;
using SightKeeper.DAL.Domain.Abstract;
using SightKeeper.UI.WPF.ViewModels.Domain;

namespace SightKeeper.UI.WPF.ViewModels.Elements;

public sealed class GeneralModelsList : IModelsListVM<IModelVM<Model>, Model>
{
	public IEnumerable<IModelVM<Model>> Models { get; }
	public ReactiveCommand<Unit, Unit> CreateNewModelCommand { get; }
	public ReactiveCommand<IModelVM<Model>, Unit> DeleteModelCommand { get; }
}