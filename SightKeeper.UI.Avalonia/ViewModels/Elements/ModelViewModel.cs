using System;
using SightKeeper.Domain.Model.Abstract;
using SightKeeper.Domain.Model.Common;
using SightKeeper.Domain.Model.Detector;

namespace SightKeeper.UI.Avalonia.ViewModels.Elements;

public interface ModelViewModel
{
	public static ModelViewModel Create(Model model)
	{
		if (model is DetectorModel detectorModel) return new DetectorModelViewModel(detectorModel);
		throw new Exception($"Unexpected type {model.GetType()}");
	}

	string Name { get; set; }
	ushort Width { get; set; }
	ushort Height { get; set; }
	ModelConfig Config { get; set; }
	Game? Game { get; set; }

	Model Model { get; }
}

public interface ModelViewModel<TModel> : ModelViewModel where TModel : Model
{
	new TModel Model { get; }
}