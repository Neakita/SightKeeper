using Avalonia;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using CommunityToolkit.Diagnostics;

namespace SightKeeper.Avalonia.Misc;

public sealed class PooledWriteableBitmap : WriteableBitmap
{
	private static readonly Vector DPI = new(96, 96);

	internal PooledWriteableBitmap(WriteableBitmapPool pool, PixelSize size, PixelFormat format) : base(size, DPI, format)
	{
		_pool = pool;
	}

	public void ReturnToPool()
	{
		if (!_isRented)
			return;
		_pool.Return(this);
		_isRented = false;
	}

	internal void MarkAsRented()
	{
		Guard.IsFalse(_isRented);
		_isRented = true;
	}

	private readonly WriteableBitmapPool _pool;
	private bool _isRented;
}