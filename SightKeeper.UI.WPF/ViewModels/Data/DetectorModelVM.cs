using SightKeeper.Backend.Data.Members.Detector;

namespace SightKeeper.UI.WPF.ViewModels.Data;

public sealed class DetectorModelVM : ModelVM
{
	private readonly DetectorModel _model;

	public DetectorModelVM(DetectorModel model) : base(model)
	{
		_model = model;
	}
}
