using CommunityToolkit.Diagnostics;

namespace SightKeeper.Domain.Model.DataSets.Classifier;

public sealed class ClassifierDataSet : DataSet<Tag, ClassifierAsset, ClassifierAssetsLibrary>
{
	public Tag CreateTag(string name, uint color)
	{
		Tag tag = new(name, color);
		AddTag(tag);
		return tag;
	}

	public override void DeleteTag(Tag tag)
	{
		bool isTagInUse = Assets.Any(asset => asset.Tag == tag);
		Guard.IsFalse(isTagInUse);
		base.DeleteTag(tag);
	}

	public ClassifierDataSet(string name, ushort resolution) : base(new ClassifierAssetsLibrary(), name, resolution)
	{
	}
}