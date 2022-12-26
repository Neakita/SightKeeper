using ReactiveUI;
using SightKeeper.Abstractions.Domain;
using SightKeeper.UI.WPF.ViewModels.Domain;
using SightKeeper.UI.WPF.ViewModels.Elements;

namespace SightKeeper.UI.WPF.ViewModels.Pages;

public sealed class ModelsPageVM : ReactiveObject
{
	public IModelsListVM<DetectorModelVM, IDetectorModel> DetectorModelsList { get; }
	

	public ModelsPageVM(IModelsListVM<DetectorModelVM, IDetectorModel> detectorModelsList)
	{
		DetectorModelsList = detectorModelsList;
	}
}
