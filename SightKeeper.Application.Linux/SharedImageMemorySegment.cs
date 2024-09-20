using System.Runtime.InteropServices;
using SightKeeper.Application.Linux.Natives;
using SightKeeper.Domain.Model;
using SixLabors.ImageSharp.PixelFormats;

namespace SightKeeper.Application.Linux;

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
		const int allPlanes = ~0;
		UIntPtr allPlanes2 = new(unchecked((uint)allPlanes));
		var drawable = (UIntPtr)LibX.XRootWindow(_display, screen);
		LibXExt.XShmGetImage(_display, drawable, _image.ximage, offset.X, offset.Y, allPlanes2);
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