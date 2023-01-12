using SightKeeper.Domain.Classifier;
using SightKeeper.UI.WPF.ViewModels.Domain;

namespace SightKeeper.UI.WPF.Misc;

public sealed class ClassifierModelToVMStrategy : IModelToVMStrategy<ClassifierModelVM, ClassifierModel>
{
	public ClassifierModelVM CreateVM(ClassifierModel model) => new(model);
}
