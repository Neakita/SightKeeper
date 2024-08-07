using FluentAssertions;
using FluentAssertions.Collections;
using SightKeeper.Domain.Model.DataSets.Assets;
using SightKeeper.Domain.Model.DataSets.Poser;
using SightKeeper.Domain.Model.DataSets.Screenshots;
using SightKeeper.Domain.Model.DataSets.Tags;
using SightKeeper.Domain.Model.DataSets.Weights;

namespace SightKeeper.Domain.Tests.DataSets;

public static class Extensions
{
	public static GenericCollectionAssertions<T> Should<T>(this ScreenshotsLibrary<T> library) where T : Screenshot
	{
		return ((IEnumerable<T>)library).Should();
	}

	public static GenericCollectionAssertions<T> Should<T>(this TagsLibrary<T> library) where T : Tag, TagsFactory<T>
	{
		return ((IEnumerable<T>)library).Should();
	}

	public static GenericCollectionAssertions<T> Should<T>(this AssetsLibrary<T> library)
		where T : Asset, AssetsFactory<T>, AssetsDestroyer<T>
	{
		return ((IEnumerable<T>)library).Should();
	}

	public static GenericCollectionAssertions<Weights<TTag>> Should<TTag>(this WeightsLibrary<TTag> library)
		where TTag : Tag, MinimumTagsCount
	{
		return ((IEnumerable<Weights<TTag>>)library).Should();
	}

	public static GenericCollectionAssertions<Weights<TTag, TKeyPointTag>> Should<TTag, TKeyPointTag>(
		this WeightsLibrary<TTag, TKeyPointTag> library)
		where TTag : Tag
		where TKeyPointTag : KeyPointTag<TTag>
	{
		return ((IEnumerable<Weights<TTag, TKeyPointTag>>)library).Should();
	}
}