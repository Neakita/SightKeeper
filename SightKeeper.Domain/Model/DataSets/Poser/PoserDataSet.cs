using CommunityToolkit.Diagnostics;

namespace SightKeeper.Domain.Model.DataSets.Poser;

public sealed class PoserDataSet : DataSet<PoserTag>
{
	public PoserAssetsLibrary Assets { get; }

	public PoserTag CreateTag(string name, uint color)
	{
		PoserTag tag = new(name, color);
		AddTag(tag);
		return tag;
	}

	public override void DeleteTag(PoserTag tag)
	{
		bool isTagInUse = Assets.SelectMany(asset => asset.Items).Any(item => item.Tag == tag);
		Guard.IsFalse(isTagInUse);
		base.DeleteTag(tag);
	}

	public PoserDataSet(string name, ushort resolution) : base(name, resolution)
	{
		Assets = new PoserAssetsLibrary();
	}
}