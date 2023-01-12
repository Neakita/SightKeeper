using SightKeeper.Domain.Detector;

namespace SightKeeper.UI.WPF.ViewModels.Domain;

public sealed class DetectorModelVM : ModelVM, IModelVM<DetectorModel>
{
	internal DetectorModelVM(DetectorModel detectorModel) : base(detectorModel) => Model = detectorModel;

	public new DetectorModel Model { get; }
}