using SightKeeper.Domain.Model.Abstract;
using SightKeeper.Domain.Model.Common;
using SightKeeper.Infrastructure.Common;

namespace SightKeeper.UI.Avalonia.ViewModels.Elements;

public interface ModelVM<out TModel> : ItemVM<TModel> where TModel : Model
{
	string Name { get; set; }
	ushort Width { get; set; }
	ushort Height { get; set; }
	ModelConfig? Config { get; set; }
	Game? Game { get; set; }

	void UpdateProperties();
}

public interface ModelVM : ModelVM<Model>
{
	public static ModelVM<TModel> Create<TModel>(TModel model) where TModel : Model =>
		Locator.Resolve<ModelVM<TModel>, TModel>(model);
}