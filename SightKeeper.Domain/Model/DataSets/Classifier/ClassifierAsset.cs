using System.Diagnostics.CodeAnalysis;
using CommunityToolkit.Diagnostics;
using SightKeeper.Domain.Model.DataSets.Assets;
using SightKeeper.Domain.Model.DataSets.Screenshots;

namespace SightKeeper.Domain.Model.DataSets.Classifier;

public sealed class ClassifierAsset : Asset, AssetsFactory<ClassifierAsset>, AssetsDestroyer<ClassifierAsset>
{
	public static ClassifierAsset Create(Screenshot<ClassifierAsset> screenshot)
	{
		return new ClassifierAsset(
			screenshot,
			(ClassifierTag)screenshot.DataSet.Tags.First(),
			(AssetsLibrary<ClassifierAsset>)screenshot.DataSet.Assets);
	}

	public static void Destroy(ClassifierAsset asset)
	{
		asset.Screenshot.SetAsset(null);
		asset.Tag.RemoveAsset(asset);
	}

	public ClassifierTag Tag
	{
		get => _tag;
		[MemberNotNull(nameof(_tag))] set
		{
			Guard.IsReferenceEqualTo(value.DataSet, DataSet);
			_tag?.RemoveAsset(this);
			_tag = value;
			_tag.AddAsset(this);
		}
	}

	public override Screenshot<ClassifierAsset> Screenshot { get; }
	public override AssetsLibrary<ClassifierAsset> Library { get; }
	public override DataSet DataSet => Library.DataSet;

	internal ClassifierAsset(Screenshot<ClassifierAsset> screenshot, ClassifierTag tag, AssetsLibrary<ClassifierAsset> library)
	{
		Screenshot = screenshot;
		Library = library;
		Tag = tag;
	}

	private ClassifierTag _tag;
}