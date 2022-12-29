using SightKeeper.DAL.Domain.Detector;
using SightKeeper.UI.WPF.ViewModels.Domain;

namespace SightKeeper.UI.WPF.Misc;

public sealed class DetectorModelToVMStrategy : IModelToVMStrategy<DetectorModelVM, DetectorModel>
{
	public DetectorModelVM ConvertToVM(DetectorModel model) => new DetectorModelVM(model);
}