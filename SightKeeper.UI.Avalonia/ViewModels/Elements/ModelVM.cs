using System;
using SightKeeper.Domain.Model.Abstract;
using SightKeeper.Domain.Model.Common;
using SightKeeper.Domain.Model.Detector;

namespace SightKeeper.UI.Avalonia.ViewModels.Elements;

public interface ModelVM
{
	public static ModelVM Create(Model model)
	{
		if (model is DetectorModel detectorModel) return new DetectorModelVM(detectorModel);
		throw new Exception($"Unexpected type {model.GetType()}");
	}

	string Name { get; set; }
	ushort Width { get; set; }
	ushort Height { get; set; }
	ModelConfig Config { get; set; }

	Model Model { get; }
}

public interface ModelVM<TModel> : ModelVM where TModel : Model
{
	new TModel Model { get; }
}