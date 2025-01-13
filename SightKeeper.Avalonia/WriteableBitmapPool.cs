using System;
using System.Collections.Concurrent;
using System.Threading;
using Avalonia;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Serilog;

namespace SightKeeper.Avalonia;

public sealed class WriteableBitmapPool : IDisposable
{
	private readonly record struct BitmapArchetype(PixelSize Size, PixelFormat? Format);

	public WriteableBitmapPool(ILogger logger)
	{
		_logger = logger;
	}

	public WriteableBitmap Rent(PixelSize size, PixelFormat? format = null)
	{
		BitmapArchetype archetype = new(size, format);
		var bag = GetOrCreateBag(archetype);
		Interlocked.Increment(ref _rented);
		if (!bag.TryTake(out var bitmap))
			bitmap = new WriteableBitmap(size, DPI, format);
		_logger.Verbose("Bitmap is rented from a pool. Total rented count: {rented}", _rented);
		return bitmap;
	}

	public void Return(WriteableBitmap bitmap)
	{
		BitmapArchetype archetype = new(bitmap.PixelSize, bitmap.Format);
		var bag = GetOrCreateBag(archetype);
		bag.Add(bitmap);
		Interlocked.Decrement(ref _rented);
		_logger.Verbose("Bitmap is returned to a pool. Total rented count: {rented}", _rented);
	}

	public void Dispose()
	{
		foreach (var bags in _bags.Values)
		foreach (var bitmap in bags)
			bitmap.Dispose();
		_bags.Clear();
	}

	private static readonly Vector DPI = new(96, 96);
	private readonly ConcurrentDictionary<BitmapArchetype, ConcurrentBag<WriteableBitmap>> _bags = new();
	private readonly ILogger _logger;
	private int _rented;

	private ConcurrentBag<WriteableBitmap> GetOrCreateBag(BitmapArchetype archetype)
	{
		return _bags.GetOrAdd(archetype, static _ => new ConcurrentBag<WriteableBitmap>());
	}
}