using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using ReactiveUI;
using SightKeeper.UI.WPF.ViewModels.Elements;

namespace SightKeeper.UI.WPF.ViewModels.Pages;

public sealed class ModelsPageVM : ReactiveObject
{
	public IEnumerable<ModelVM> Models { get; } = Enumerable.Empty<ModelVM>();
	
	
	public ReactiveCommand<ModelVM, Unit> DeleteModelCommand { get; }


	public ModelsPageVM()
	{
		DeleteModelCommand = ReactiveCommand.Create<ModelVM>(DeleteModel);
	}


	private void DeleteModel(ModelVM model)
	{
		
	}
}
