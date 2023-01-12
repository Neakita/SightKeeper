using System.Collections.Generic;
using System.Reactive;
using ReactiveUI;
using SightKeeper.Domain.Common;

namespace SightKeeper.UI.WPF.ViewModels.Domain;

public sealed class ItemClassesVM : ReactiveObject
{
	public IEnumerable<ItemClass> ItemClasses => _modelVM.Classes;

	public ReactiveCommand<Unit, Unit> CreateItemClassCommand =>
		_createItemClassCommand ??= ReactiveCommand.Create(CreateItemClass);


	public ItemClassesVM(ModelVM modelVM)
	{
		_modelVM = modelVM;
	}


	private void CreateItemClass()
	{
		
	}
	

	private readonly ModelVM _modelVM;

	private ReactiveCommand<Unit, Unit>? _createItemClassCommand;
}
