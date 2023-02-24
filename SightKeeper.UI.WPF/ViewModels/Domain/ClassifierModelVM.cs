using SightKeeper.Domain.Model.Classifier;

namespace SightKeeper.UI.WPF.ViewModels.Domain;

public sealed class ClassifierModelVM : ModelVM, IModelVM<ClassifierModel>
{
	public ClassifierModelVM(ClassifierModel model) : base(model)
	{
		Model = model;
	}

	public new ClassifierModel Model { get; }
}
