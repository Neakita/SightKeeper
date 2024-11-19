using SightKeeper.Application;
using SightKeeper.Domain.Model.DataSets.Classifier;

namespace SightKeeper.Avalonia.Annotation.Assets;

internal sealed class ClassifierAssetViewModel : AssetViewModel<ClassifierAsset>
{
	public ClassifierTag Tag
	{
		get => Value.Tag;
		set => SetProperty(Tag, value, this, (viewModel, newValue) => viewModel._annotator.SetTag(viewModel.Value, newValue));
	}

	public ClassifierAssetViewModel(ClassifierAsset value, ClassifierAnnotator annotator) : base(value)
	{
		_annotator = annotator;
	}

	private readonly ClassifierAnnotator _annotator;
}