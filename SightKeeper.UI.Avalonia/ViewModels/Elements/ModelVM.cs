using System.Collections.Generic;
using SightKeeper.Domain.Model.Abstract;
using SightKeeper.Domain.Model.Common;
using SightKeeper.Infrastructure.Common;

namespace SightKeeper.UI.Avalonia.ViewModels.Elements;

public interface ModelVM : ItemVM<Model>
{
	public static ModelVM Create<TModel>(TModel model) where TModel : Model =>
		Locator.Resolve<ModelVM, TModel>(model);
	
	string Name { get; set; }
	ushort Width { get; set; }
	ushort Height { get; set; }
	ICollection<ItemClass> ItemClasses { get; }
	Game? Game { get; set; }
	ModelConfig? Config { get; set; }

	void UpdateProperties();
}