using SightKeeper.Abstractions.Domain;

namespace SightKeeper.UI.WPF.ViewModels.Domain;

public sealed class DetectorModelVM : IModelVM<IDetectorModel>
{
	public IDetectorModel Model { get; }
	public string Name => Model.Name;


	internal DetectorModelVM(IDetectorModel detectorModel)
	{
		Model = detectorModel;
	}
}
