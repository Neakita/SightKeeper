using System;
using System.Collections.Concurrent;
using Avalonia;
using Avalonia.Media.Imaging;
using Avalonia.Platform;

namespace SightKeeper.Avalonia;

internal sealed class WriteableBitmapPool : IDisposable
{
	private readonly record struct BitmapArchetype(PixelSize Size, PixelFormat? Format);

	public WriteableBitmap Rent(PixelSize size, PixelFormat? format = null)
	{
		BitmapArchetype archetype = new(size, format);
		var bag = GetOrCreateBag(archetype);
		if (bag.TryTake(out var bitmap))
			return bitmap;
		return new WriteableBitmap(size, DPI, format);
	}

	public void Return(WriteableBitmap bitmap)
	{
		BitmapArchetype archetype = new(bitmap.PixelSize, bitmap.Format);
		var bag = GetOrCreateBag(archetype);
		bag.Add(bitmap);
	}

	private static readonly Vector DPI = new(96, 96);
	private readonly ConcurrentDictionary<BitmapArchetype, ConcurrentBag<WriteableBitmap>> _bags = new();

	private ConcurrentBag<WriteableBitmap> GetOrCreateBag(BitmapArchetype archetype)
	{
		return _bags.GetOrAdd(archetype, static _ => new ConcurrentBag<WriteableBitmap>());
	}

	public void Dispose()
	{
		foreach (var bags in _bags.Values)
		foreach (var bitmap in bags)
			bitmap.Dispose();
		_bags.Clear();
	}
}