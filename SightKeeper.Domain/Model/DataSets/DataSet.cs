using CommunityToolkit.Diagnostics;
using SightKeeper.Domain.Model.DataSets.Poser;

namespace SightKeeper.Domain.Model.DataSets;

public abstract class DataSet
{
	public string Name { get; set; }
	public string Description { get; set; }
	public Game? Game { get; set; }
	public ushort Resolution { get; }
	public abstract TagsLibrary Tags { get; }
	public abstract ScreenshotsLibrary Screenshots { get; }
	public abstract AssetsLibrary Assets { get; }
	public abstract WeightsLibrary Weights { get; }

	public override string ToString() => Name;

	protected DataSet(string name, ushort resolution)
	{
		Name = name;
		Description = string.Empty;
		Guard.IsGreaterThan<int>(resolution, 0);
		Guard.IsEqualTo(resolution % 32, 0);
		
		Resolution = resolution;
	}
}

public class DataSet<TTag, TAsset> : DataSet
	where TTag : Tag, TagsFactory<TTag>, MinimumTagsCount
	where TAsset : Asset, AssetsFactory<TAsset>, AssetsDestroyer<TAsset>
{
	public override TagsLibrary<TTag> Tags { get; }
	public override AssetScreenshotsLibrary<TAsset> Screenshots { get; }
	public override AssetsLibrary<TAsset> Assets { get; }
	public override WeightsLibrary<TTag> Weights { get; }

	public DataSet(string name, ushort resolution) : base(name, resolution)
	{
		Tags = new TagsLibrary<TTag>(this);
		Screenshots = new AssetScreenshotsLibrary<TAsset>(this);
		Assets = new AssetsLibrary<TAsset>(this);
		Weights = new WeightsLibrary<TTag>(this);
	}
}

public class DataSet<TTag, TKeyPointTag, TAsset> : DataSet
	where TTag : Tag, TagsFactory<TTag>
	where TKeyPointTag : KeyPointTag<TTag>
	where TAsset : Asset, AssetsFactory<TAsset>, AssetsDestroyer<TAsset>
{
	public override TagsLibrary<TTag> Tags { get; }
	public override AssetScreenshotsLibrary<TAsset> Screenshots { get; }
	public override AssetsLibrary<TAsset> Assets { get; }
	public override WeightsLibrary<TTag, TKeyPointTag> Weights { get; }

	public DataSet(string name, ushort resolution) : base(name, resolution)
	{
		Tags = new TagsLibrary<TTag>(this);
		Screenshots = new AssetScreenshotsLibrary<TAsset>(this);
		Assets = new AssetsLibrary<TAsset>(this);
		Weights = new WeightsLibrary<TTag, TKeyPointTag>(this);
	}
}