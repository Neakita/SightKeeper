using SightKeeper.Domain.DataSets.Classifier;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Avalonia.Annotation.Assets;

internal sealed class ClassifierAssetViewModel : AssetViewModel<ClassifierAsset>, AssetViewModelFactory<ClassifierAssetViewModel, ClassifierAsset>
{
	public static ClassifierAssetViewModel Create(ClassifierAsset value)
	{
		return new ClassifierAssetViewModel(value);
	}

	public Tag Tag => Value.Tag;

	public ClassifierAssetViewModel(ClassifierAsset value) : base(value)
	{
	}

	internal void NotifyTagChanged() => OnPropertyChanged(nameof(Tag));
}