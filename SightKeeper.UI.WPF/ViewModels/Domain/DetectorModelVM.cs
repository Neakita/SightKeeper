using SightKeeper.DAL.Domain.Detector;

namespace SightKeeper.UI.WPF.ViewModels.Domain;

public sealed class DetectorModelVM : IModelVM<DetectorModel>
{
	public DetectorModel Model { get; }
	public string Name => Model.Name;


	internal DetectorModelVM(DetectorModel detectorModel)
	{
		Model = detectorModel;
	}
}
