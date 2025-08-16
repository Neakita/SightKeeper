using System;
using System.Collections.Concurrent;
using System.Threading;
using Avalonia;
using Avalonia.Platform;
using Serilog;

namespace SightKeeper.Avalonia.Misc;

public sealed class WriteableBitmapPool : IDisposable
{
	private readonly record struct BitmapArchetype(PixelSize Size, PixelFormat? Format);

	public PooledWriteableBitmap Rent(PixelSize size, PixelFormat format)
	{
		BitmapArchetype archetype = new(size, format);
		var bag = GetOrCreateBag(archetype);
		if (!bag.TryTake(out var bitmap))
		{
			bitmap = new PooledWriteableBitmap(this, size, format);
			Logger.Verbose("New Bitmap created with size {size} and format {format}", size, format);
		}

		bitmap.MarkAsRented();
		Interlocked.Increment(ref _rented);
		Logger.Verbose("Bitmap is rented from a pool. Total rented count: {rented}", _rented);
		return bitmap;
	}

	public void Dispose()
	{
		foreach (var bags in _bags.Values)
		foreach (var bitmap in bags)
			bitmap.Dispose();
		_bags.Clear();
	}

	internal void Return(PooledWriteableBitmap bitmap)
	{
		BitmapArchetype archetype = new(bitmap.PixelSize, bitmap.Format);
		var bag = GetOrCreateBag(archetype);
		bag.Add(bitmap);
		Interlocked.Decrement(ref _rented);
		Logger.Verbose("Bitmap is returned to a pool. Total rented count: {rented}", _rented);
	}

	private readonly ConcurrentDictionary<BitmapArchetype, ConcurrentBag<PooledWriteableBitmap>> _bags = new();
	private static readonly ILogger Logger = Log.ForContext<WriteableBitmapPool>();
	private int _rented;

	private ConcurrentBag<PooledWriteableBitmap> GetOrCreateBag(BitmapArchetype archetype)
	{
		return _bags.GetOrAdd(archetype, static _ => new ConcurrentBag<PooledWriteableBitmap>());
	}
}