using SightKeeper.Domain.Model.DataSets.Assets;
using SightKeeper.Domain.Model.DataSets.Poser;
using SightKeeper.Domain.Model.DataSets.Screenshots;
using SightKeeper.Domain.Model.DataSets.Tags;
using SightKeeper.Domain.Model.DataSets.Weights;

namespace SightKeeper.Domain.Model.DataSets;

public abstract class DataSet
{
	public string Name { get; set; } = string.Empty;
	public string Description { get; set; } = string.Empty;
	public Game? Game { get; set; }

	public abstract TagsLibrary TagsLibrary { get; }
	public abstract ScreenshotsLibrary ScreenshotsLibrary { get; }
	public abstract AssetsLibrary AssetsLibrary { get; }
	public abstract WeightsLibrary WeightsLibrary { get; }
}

public class DataSet<TTag, TAsset> : DataSet
	where TTag : Tag, TagsFactory<TTag>, MinimumTagsCount
	where TAsset : Asset, AssetsFactory<TAsset>, AssetsDestroyer<TAsset>
{
	public override TagsLibrary<TTag> TagsLibrary { get; }
	public override ScreenshotsLibrary<TAsset> ScreenshotsLibrary { get; }
	public override AssetsLibrary<TAsset> AssetsLibrary { get; }
	public override WeightsLibrary<TTag> WeightsLibrary { get; }

	protected DataSet()
	{
		TagsLibrary = new TagsLibrary<TTag>(this);
		ScreenshotsLibrary = new ScreenshotsLibrary<TAsset>(this);
		AssetsLibrary = new AssetsLibrary<TAsset>(this);
		WeightsLibrary = new WeightsLibrary<TTag>(this);
	}
}

public class DataSet<TTag, TKeyPointTag, TAsset> : DataSet
	where TTag : PoserTag, TagsFactory<TTag>
	where TKeyPointTag : KeyPointTag<TTag>
	where TAsset : Asset, AssetsFactory<TAsset>, AssetsDestroyer<TAsset>
{
	public override TagsLibrary<TTag> TagsLibrary { get; }
	public override ScreenshotsLibrary<TAsset> ScreenshotsLibrary { get; }
	public override AssetsLibrary<TAsset> AssetsLibrary { get; }
	public override WeightsLibrary<TTag, TKeyPointTag> WeightsLibrary { get; }

	protected DataSet()
	{
		TagsLibrary = new TagsLibrary<TTag>(this);
		ScreenshotsLibrary = new ScreenshotsLibrary<TAsset>(this);
		AssetsLibrary = new AssetsLibrary<TAsset>(this);
		WeightsLibrary = new WeightsLibrary<TTag, TKeyPointTag>(this);
	}
}