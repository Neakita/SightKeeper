using System.Buffers;
using System.Diagnostics;
using CommunityToolkit.Diagnostics;
using SharpDX;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using SightKeeper.Domain.Model;
using SixLabors.ImageSharp.PixelFormats;
using Device = SharpDX.Direct3D11.Device;
using Image = SixLabors.ImageSharp.Image;
using MapFlags = SharpDX.Direct3D11.MapFlags;
using ResultCode = SharpDX.DXGI.ResultCode;

namespace SightKeeper.Application.Windows;

public class DX11ScreenCapture : ScreenCapture, IDisposable
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
		_size = new Vector2<int>(width, height);
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
		_buffer = ArrayPool<Bgra32>.Shared.Rent(_size.X * _size.Y);
		Task.Factory.StartNew(NewMethod, TaskCreationOptions.LongRunning);
		Directory.CreateDirectory("captures");
	}

	private readonly Output1 _output1;
	private readonly Device _device;
	private readonly Texture2D _screenTexture;
	private readonly Vector2<int> _size;
	private readonly CancellationTokenSource _cancellationTokenSource = new();
	private readonly Bgra32[] _buffer;
	private DateTime _lastFpsLogTime;
	private int _capturesCount;
	private readonly Factory1 _factory;

	private void NewMethod()
	{
		var duplicatedOutput = _output1.DuplicateOutput(_device);
		try
		{
			while (!_cancellationTokenSource.IsCancellationRequested)
			{
				try
				{
					var result = NewMethod1(duplicatedOutput);
					if (result == -2005270490)
					{
						duplicatedOutput.Dispose();
						duplicatedOutput = _output1.DuplicateOutput(_device);
					}
				}
				catch (SharpDXException e)
				{
					if (e.ResultCode.Code != ResultCode.WaitTimeout.Result.Code)
					{
						Trace.TraceError(e.Message);
						Trace.TraceError(e.StackTrace);
					}
				}
			}
		}
		finally
		{
			duplicatedOutput.Dispose();
		}
	}

	private unsafe int NewMethod1(OutputDuplication duplicatedOutput)
	{
		var result = duplicatedOutput.TryAcquireNextFrame(-1, out _, out var screenResource);
		if (result.Failure)
		{
			return result.Code;
		}
		using (var screenTexture2D = screenResource.QueryInterface<Texture2D>())
			_device.ImmediateContext.CopyResource(screenTexture2D, _screenTexture);
		var mapSource = _device.ImmediateContext.MapSubresource(_screenTexture, 0, MapMode.Read, MapFlags.None);
		Guard.IsEqualTo(mapSource.RowPitch, _size.X * 4);
		Span<Bgra32> sourceSpan = new((void*)mapSource.DataPointer, _size.X * _size.Y);
		sourceSpan.CopyTo(_buffer);
		_device.ImmediateContext.UnmapSubresource(_screenTexture, 0);
		screenResource.Dispose();
		duplicatedOutput.ReleaseFrame();
		_capturesCount++;
		var now = DateTime.UtcNow;
		var passed = now - _lastFpsLogTime;
		if (passed.TotalSeconds >= 1)
		{
			Console.WriteLine($"FPS: {((float)_capturesCount / passed.TotalSeconds):G2}");
			_capturesCount = 0;
			_lastFpsLogTime = now;
		}

		return 0;
	}

	public void Stop()
	{
		_cancellationTokenSource.Cancel();
	}

	public Image Capture(Vector2<ushort> resolution, Vector2<ushort> offset)
	{
		return Image.LoadPixelData<Bgra32>(_buffer, _size.X, _size.Y);
	}

	public void Dispose()
	{
		Stop();
		_output1.Dispose();
		_device.Dispose();
		_screenTexture.Dispose();
		_cancellationTokenSource.Dispose();
	}
}