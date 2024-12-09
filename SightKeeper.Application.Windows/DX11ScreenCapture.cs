using CommunityToolkit.Diagnostics;
using CommunityToolkit.HighPerformance;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using SightKeeper.Application.Screenshotting;
using SightKeeper.Domain;
using SixLabors.ImageSharp.PixelFormats;
using Yolo;
using Device = SharpDX.Direct3D11.Device;
using MapFlags = SharpDX.Direct3D11.MapFlags;
using Resource = SharpDX.DXGI.Resource;

namespace SightKeeper.Application.Windows;

public sealed class DX11ScreenCapture : ScreenCapture<Bgra32>, IDisposable
{
	public DX11ScreenCapture()
	{
		_factory = new Factory1();
		var adapter = _factory.GetAdapter1(0);
		_device = new Device(adapter);
		using(var multiThread = _device.QueryInterface<Multithread>())
		{
			multiThread.SetMultithreadProtected(true);
		}
		var output = adapter.GetOutput(0);
		_output1 = output.QueryInterface<Output1>();
		int width = output.Description.DesktopBounds.Right;
		int height = output.Description.DesktopBounds.Bottom;
		_size = new Vector2D<int>(width, height);
		Texture2DDescription textureDesc = new()
		{
			CpuAccessFlags = CpuAccessFlags.Read,
			BindFlags = BindFlags.None,
			Format = Format.B8G8R8A8_UNorm,
			Width = width,
			Height = height,
			OptionFlags = ResourceOptionFlags.None,
			MipLevels = 1,
			ArraySize = 1,
			SampleDescription = { Count = 1, Quality = 0 },
			Usage = ResourceUsage.Staging
		};
		_screenTexture = new Texture2D(_device, textureDesc);
		_outputDuplication = _output1.DuplicateOutput(_device);
	}

	private readonly Output1 _output1;
	private readonly Device _device;
	private readonly Texture2D _screenTexture;
	private readonly Vector2D<int> _size;
	private readonly Factory1 _factory;
	private readonly OutputDuplication _outputDuplication;
	private Resource? _screenResource;

	private unsafe ReadOnlySpan2D<Bgra32> Capture()
	{
		if (_screenResource != null)
		{
			_device.ImmediateContext.UnmapSubresource(_screenTexture, 0);
			_screenResource.Dispose();
			_outputDuplication.ReleaseFrame();
		}
		_outputDuplication.TryAcquireNextFrame(1000, out _, out _screenResource).CheckError();
		using (var screenTexture2D = _screenResource.QueryInterface<Texture2D>())
			_device.ImmediateContext.CopyResource(screenTexture2D, _screenTexture);
		var mapSource = _device.ImmediateContext.MapSubresource(_screenTexture, 0, MapMode.Read, MapFlags.None);
		Guard.IsEqualTo(mapSource.RowPitch, _size.X * 4);
		return new ReadOnlySpan2D<Bgra32>((void*)mapSource.DataPointer, _size.Y, _size.X, 0);
	}

	public ReadOnlySpan2D<Bgra32> Capture(Vector2<ushort> resolution, Vector2<ushort> offset)
	{
		return Capture().Slice(offset.Y, offset.X, resolution.Y, resolution.X);
	}

	public void Dispose()
	{
		_output1.Dispose();
		_device.Dispose();
		_outputDuplication.Dispose();
		_screenTexture.Dispose();
		_factory.Dispose();
	}
}