using System.Numerics.Tensors;
using System.Runtime.InteropServices;
using SightKeeper.Application.Linux.X11.Natives;
using SightKeeper.Domain.Model;
using SixLabors.ImageSharp.PixelFormats;

namespace SightKeeper.Application.Linux.X11;

internal sealed class SharedImageMemorySegment<TPixel> : IDisposable
	where TPixel : unmanaged, IPixel<TPixel>
{
	public Vector2<ushort> Resolution { get; }
	public unsafe ReadOnlySpan<TPixel> Data => new(_image.data, Resolution.X * Resolution.Y);

	public unsafe SharedImageMemorySegment(nint display, Vector2<ushort> resolution)
	{
		_display = display;
		Resolution = resolution;
		_image = new ShmImage();
		_handle = GCHandle.Alloc(this, GCHandleType.Pinned);
		fixed (ShmImage* image = &_image)
			XLibShm.CreateImageSharedMemorySegment(display, image, resolution.X, resolution.Y);
	}

	public unsafe void FetchData(int screen, Vector2<ushort> offset)
	{
		const ulong allPlanes = unchecked((ulong)~0);
		var drawable = (UIntPtr)LibX.XRootWindow(_display, screen);
		LibXExt.XShmGetImage(_display, drawable, _image.ximage, offset.X, offset.Y, allPlanes);
		
		// xlib doesn't use most significant byte but fills it with zeros
		// because of that ImageSharp treats it as fully transparent Bgra32
		Span<uint> span = new(_image.data, Resolution.X * Resolution.Y);
		TensorPrimitives.BitwiseOr(span, 0xFF_00_00_00, span);
	}

	public void Dispose()
	{
		ReleaseUnmanagedResources();
		GC.SuppressFinalize(this);
	}

	private readonly IntPtr _display;
	private readonly ShmImage _image;
	private GCHandle _handle;

	private unsafe void ReleaseUnmanagedResources()
	{
		fixed (ShmImage* image = &_image)
			XLibShm.DestroyImage(_display, image);
		_handle.Free();
	}

	~SharedImageMemorySegment()
	{
		ReleaseUnmanagedResources();
	}
}