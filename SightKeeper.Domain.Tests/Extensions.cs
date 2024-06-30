using FluentAssertions;
using FluentAssertions.Collections;
using SightKeeper.Domain.Model.DataSets;

namespace SightKeeper.Domain.Tests;

public static class Extensions
{
	public static GenericCollectionAssertions<T> Should<T>(this ScreenshotsLibrary<T> library) where T : Screenshot
	{
		return ((IEnumerable<T>)library).Should();
	}
	public static GenericCollectionAssertions<T> Should<T>(this TagsLibrary<T> library) where T : Tag
	{
		return ((IEnumerable<T>)library).Should();
	}
	public static GenericCollectionAssertions<T> Should<T>(this AssetsLibrary<T> library) where T : Asset
	{
		return ((IEnumerable<T>)library).Should();
	}
	public static GenericCollectionAssertions<T> Should<T>(this WeightsLibrary<T> library) where T : Weights
	{
		return ((IEnumerable<T>)library).Should();
	}
}