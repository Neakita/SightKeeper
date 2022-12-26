using System.Collections.Generic;
using System.Reactive;
using ReactiveUI;
using SightKeeper.Abstractions.Domain;
using SightKeeper.UI.WPF.ViewModels.Domain;

namespace SightKeeper.UI.WPF.ViewModels.Elements;

public sealed class GeneralModelsList : IModelsListVM<IModelVM<IModel>, IModel>
{
	public IEnumerable<IModelVM<IModel>> Models { get; }
	public ReactiveCommand<Unit, Unit> CreateNewModelCommand { get; }
	public ReactiveCommand<IModelVM<IModel>, Unit> DeleteModelCommand { get; }
}
