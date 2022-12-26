using SightKeeper.Abstractions.Domain;
using SightKeeper.UI.WPF.ViewModels.Domain;

namespace SightKeeper.UI.WPF.Misc;

public sealed class DetectorModelToVMStrategy : IModelToVMStrategy<DetectorModelVM, IDetectorModel>
{
	public DetectorModelVM ConvertToVM(IDetectorModel model) => new(model);
}
