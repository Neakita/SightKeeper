using SightKeeper.UI.WPF.ViewModels.Domain;

namespace SightKeeper.UI.WPF.Misc;

public sealed class DetectorModelsListInfo : IModelsListInfo<DetectorModelVM>
{
	public string Label => "Detectors";
}