using SightKeeper.Domain.Model.DataSets.Classifier;

namespace SightKeeper.Avalonia.Annotation.Assets;

internal sealed class ClassifierAssetViewModel : AssetViewModel<ClassifierAsset>, AssetViewModelFactory<ClassifierAssetViewModel, ClassifierAsset>
{
	public static ClassifierAssetViewModel Create(ClassifierAsset value)
	{
		return new ClassifierAssetViewModel(value);
	}

	public ClassifierTag Tag => Value.Tag;

	public ClassifierAssetViewModel(ClassifierAsset value) : base(value)
	{
	}

	internal void NotifyTagChanged() => OnPropertyChanged(nameof(Tag));
}