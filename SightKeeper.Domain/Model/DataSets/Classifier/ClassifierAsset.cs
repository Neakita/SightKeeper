using System.Diagnostics.CodeAnalysis;
using CommunityToolkit.Diagnostics;

namespace SightKeeper.Domain.Model.DataSets.Classifier;

public sealed class ClassifierAsset : Asset
{
	public ClassifierTag Tag
	{
		get => _tag;
		[MemberNotNull(nameof(_tag))] set
		{
			Guard.IsReferenceEqualTo(value.DataSet, DataSet);
			_tag = value;
		}
	}

	public ClassifierScreenshot Screenshot { get; }
	public ClassifierAssetsLibrary Library { get; }
	public ClassifierDataSet DataSet => Library.DataSet;

	internal ClassifierAsset(ClassifierScreenshot screenshot, ClassifierTag tag, ClassifierAssetsLibrary library)
	{
		Screenshot = screenshot;
		Library = library;
		Tag = tag;
	}

	private ClassifierTag _tag;
}