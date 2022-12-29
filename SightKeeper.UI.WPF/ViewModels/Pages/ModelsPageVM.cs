using ReactiveUI;
using SightKeeper.DAL.Domain.Detector;
using SightKeeper.UI.WPF.ViewModels.Domain;
using SightKeeper.UI.WPF.ViewModels.Elements;

namespace SightKeeper.UI.WPF.ViewModels.Pages;

public sealed class ModelsPageVM : ReactiveObject
{
	public IModelsListVM<DetectorModelVM, DetectorModel> DetectorModelsList { get; }
	

	public ModelsPageVM(IModelsListVM<DetectorModelVM, DetectorModel> detectorModelsList)
	{
		DetectorModelsList = detectorModelsList;
	}
}
