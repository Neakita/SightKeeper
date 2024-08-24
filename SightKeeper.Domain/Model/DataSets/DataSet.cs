using CommunityToolkit.Diagnostics;
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

	public ushort Resolution
	{
		get => _resolution;
		init
		{
			Guard.IsGreaterThan<int>(value, 0);
			Guard.IsEqualTo(value % 32, 0);
			_resolution = value;
		}
	}

	public abstract TagsLibrary Tags { get; }
	public abstract ScreenshotsLibrary Screenshots { get; }
	public abstract AssetsLibrary Assets { get; }
	public abstract WeightsLibrary Weights { get; }

	public override string ToString() => Name;

	private readonly ushort _resolution = 320;
}

public class DataSet<TTag, TAsset> : DataSet
	where TTag : Tag, TagsFactory<TTag>, MinimumTagsCount
	where TAsset : Asset, AssetsFactory<TAsset>, AssetsDestroyer<TAsset>
{
	public override TagsLibrary<TTag> Tags { get; }
	public override ScreenshotsLibrary<TAsset> Screenshots { get; }
	public override AssetsLibrary<TAsset> Assets { get; }
	public override WeightsLibrary<TTag> Weights { get; }

	protected DataSet()
	{
		Tags = new TagsLibrary<TTag>(this);
		Screenshots = new ScreenshotsLibrary<TAsset>(this);
		Assets = new AssetsLibrary<TAsset>(this);
		Weights = new WeightsLibrary<TTag>(this);
	}
}

public class DataSet<TTag, TKeyPointTag, TAsset> : DataSet
	where TTag : PoserTag, TagsFactory<TTag>
	where TKeyPointTag : KeyPointTag<TTag>
	where TAsset : Asset, AssetsFactory<TAsset>, AssetsDestroyer<TAsset>
{
	public override TagsLibrary<TTag> Tags { get; }
	public override ScreenshotsLibrary<TAsset> Screenshots { get; }
	public override AssetsLibrary<TAsset> Assets { get; }
	public override WeightsLibrary<TTag, TKeyPointTag> Weights { get; }

	protected DataSet()
	{
		Tags = new TagsLibrary<TTag>(this);
		Screenshots = new ScreenshotsLibrary<TAsset>(this);
		Assets = new AssetsLibrary<TAsset>(this);
		Weights = new WeightsLibrary<TTag, TKeyPointTag>(this);
	}
}