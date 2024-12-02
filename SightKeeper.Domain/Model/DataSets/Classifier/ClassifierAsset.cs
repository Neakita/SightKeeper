using System.Diagnostics.CodeAnalysis;
using CommunityToolkit.Diagnostics;
using SightKeeper.Domain.Model.DataSets.Assets;
using SightKeeper.Domain.Model.DataSets.Screenshots;

namespace SightKeeper.Domain.Model.DataSets.Classifier;

public sealed class ClassifierAsset : Asset
{
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
	public override ClassifierAssetsLibrary Library { get; }
	public override ClassifierDataSet DataSet => Library.DataSet;

	internal ClassifierAsset(Screenshot<ClassifierAsset> screenshot, ClassifierTag tag, ClassifierAssetsLibrary library)
	{
		Screenshot = screenshot;
		Library = library;
		Tag = tag;
	}

	private ClassifierTag _tag;
}