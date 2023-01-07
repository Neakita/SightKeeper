using SightKeeper.UI.WPF.ViewModels.Domain;

namespace SightKeeper.UI.WPF.ViewModels.Dialogs;

public sealed class ModelEditorVM
{
	public ModelVM ModelVM { get; }
	public DetectorModelVM? DetectorModelVM { get; }
	public ClassifierModelVM? ClassifierModelVM { get; }
	
	
	public ModelEditorVM(ModelVM modelVM)
	{
		ModelVM = modelVM;
		DetectorModelVM = modelVM as DetectorModelVM;
		ClassifierModelVM = modelVM as ClassifierModelVM;
	}
}
