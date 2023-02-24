using SightKeeper.Domain.Model.Detector;
using SightKeeper.UI.WPF.ViewModels.Domain;

namespace SightKeeper.UI.WPF.Misc;

public sealed class DetectorModelToVMStrategy : IModelToVMStrategy<DetectorModelVM, DetectorModel>
{
	public DetectorModelVM CreateVM(DetectorModel model) => new(model);
}