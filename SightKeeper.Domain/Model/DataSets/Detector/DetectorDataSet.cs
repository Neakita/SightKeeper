using CommunityToolkit.Diagnostics;

namespace SightKeeper.Domain.Model.DataSets.Detector;

public sealed class DetectorDataSet : DataSet<Tag>
{
	public DetectorAssetsLibrary Assets { get; }

	public Tag CreateTag(string name, uint color)
	{
		Tag tag = new(name, color);
		AddTag(tag);
		return tag;
	}

	public override void DeleteTag(Tag tag)
	{
		bool isTagInUse = Assets.SelectMany(asset => asset.Items).Any(item => item.Tag == tag);
		Guard.IsFalse(isTagInUse);
		base.DeleteTag(tag);
	}

	public DetectorDataSet(string name, ushort resolution) : base(name, resolution)
	{
		Assets = new DetectorAssetsLibrary();
	}
}