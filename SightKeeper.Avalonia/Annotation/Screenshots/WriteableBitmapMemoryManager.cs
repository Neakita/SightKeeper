using System;
using System.Buffers;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using CommunityToolkit.Diagnostics;

namespace SightKeeper.Avalonia.Annotation.Screenshots;

internal sealed class WriteableBitmapMemoryManager<T> : MemoryManager<T> where T : unmanaged
{
	public WriteableBitmapMemoryManager(WriteableBitmap bitmap)
	{
		_bitmap = bitmap;
		_lockedFramebuffer = _bitmap.Lock();
	}

	public override unsafe Span<T> GetSpan()
	{
		ObjectDisposedException.ThrowIf(_lockedFramebuffer == null, this);
		Guard.IsNotNull(_bitmap.Format);
		Guard.IsEqualTo(sizeof(T) * 8, _bitmap.Format.Value.BitsPerPixel);
		return new Span<T>((void*)_lockedFramebuffer.Address, _bitmap.PixelSize.Width * _bitmap.PixelSize.Height);
	}

	public override MemoryHandle Pin(int elementIndex = 0)
	{
		throw new NotSupportedException();
	}

	public override void Unpin()
	{
	}

	protected override void Dispose(bool disposing)
	{
		_lockedFramebuffer?.Dispose();
	}

	private readonly WriteableBitmap _bitmap;
	private readonly ILockedFramebuffer? _lockedFramebuffer;
}