﻿using System.Collections.Immutable;
using CommunityToolkit.Diagnostics;

namespace SightKeeper.Domain.Model.DataSets.Poser2D;

public sealed class Poser2DAsset : ItemsAsset<Poser2DItem>, AssetsFactory<Poser2DAsset>, AssetsDestroyer<Poser2DAsset>
{
	public static Poser2DAsset Create(Screenshot<Poser2DAsset> screenshot)
	{
		Poser2DAsset asset = new(screenshot, (AssetsLibrary<Poser2DAsset>)screenshot.DataSet.Assets);
		screenshot.SetAsset(asset);
		return asset;
	}

	public static void Destroy(Poser2DAsset asset)
	{
		asset.Screenshot.SetAsset(null);
		foreach (var item in asset.Items)
			item.Tag.RemoveItem(item);
	}

	public override Screenshot<Poser2DAsset> Screenshot { get; }
	public override AssetsLibrary<Poser2DAsset> Library { get; }
	public override DataSet DataSet => Library.DataSet;

	public Poser2DItem CreateItem(Poser2DTag tag, Bounding bounding, IReadOnlyCollection<Vector2<double>> keyPoints, ImmutableList<double> properties)
	{
		Guard.IsEqualTo(keyPoints.Count, tag.KeyPoints.Count);
		Guard.IsEqualTo(properties.Count, tag.Properties.Count);
		var taggedKeyPoints = keyPoints.Zip(tag.KeyPoints).Select(t => new KeyPoint2D(t.First, t.Second)).ToList();
		Poser2DItem item = new(tag, bounding, taggedKeyPoints, properties, this);
		AddItem(item);
		return item;
	}

	public override void DeleteItem(Poser2DItem item)
	{
		base.DeleteItem(item);
		item.Tag.RemoveItem(item);
	}

	public override void ClearItems()
	{
		foreach (var item in Items)
			item.Tag.RemoveItem(item);
		base.ClearItems();
	}

	internal Poser2DAsset(Screenshot<Poser2DAsset> screenshot, AssetsLibrary<Poser2DAsset> library)
	{
		Screenshot = screenshot;
		Library = library;
	}
}