using System.Drawing;
using System.Drawing.Imaging;
using CommunityToolkit.Diagnostics;
using Serilog;
using SerilogTimings;
using SharpDX;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using SightKeeper.Application;
using SightKeeper.Domain.Model.Common;
using Device = SharpDX.Direct3D11.Device;
using Image = System.Drawing.Image;

namespace SightKeeper.Services.Windows;

public sealed class SharpDXScreenCapture : ScreenCapture
{
	private readonly Adapter1 _adapter;
	private Device _device;
	private Output _output;
	private Output1 _output1;

	public SharpDXScreenCapture()
	{
		var factory = new Factory1();
		_adapter = factory.GetAdapter1(0);
		Log.Debug("Adapter description: {Description}", _adapter.Description1.Description);
		_device = new Device(_adapter);
		_output = _adapter.GetOutput(0);
		Log.Debug("Device name: {Description}", _output.Description.DeviceName);
		_output1 = _output.QueryInterface<Output1>();
	}
	
	public byte[] Capture()
	{
		Guard.IsNotNull(Resolution);
		using Operation operation = Operation.Begin("Screen capturing via DirectX");
		int width = _output.Description.DesktopBounds.Right;
        int height = _output.Description.DesktopBounds.Bottom;
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
        using Texture2D screenTexture = new(_device, textureDesc);
        using OutputDuplication duplicatedOutput = _output1.DuplicateOutput(_device);
        Thread.Sleep(20); // захватчику экрана надо время проинициализироваться
        Bitmap bmp = new(width, height, PixelFormat.Format32bppArgb);
        SharpDX.DXGI.Resource? screenResource = null;
        try
        {
            if (duplicatedOutput.TryAcquireNextFrame(10, out _, out screenResource) != Result.Ok)
            {
	            operation.Complete();
	            return ImageToBytes(bmp);
            }

            using (Texture2D screenTexture2D = screenResource.QueryInterface<Texture2D>())
            {
                _device.ImmediateContext.CopyResource(screenTexture2D, screenTexture);
            }

            DataBox mapSource = _device.ImmediateContext.MapSubresource(screenTexture, 0, MapMode.Read, SharpDX.Direct3D11.MapFlags.None);
            BitmapData bmpData = bmp.LockBits(new Rectangle(Point.Empty, bmp.Size), ImageLockMode.WriteOnly, bmp.PixelFormat);
            nint sourcePtr = mapSource.DataPointer;
            nint destPtr = bmpData.Scan0;
            Utilities.CopyMemory(destPtr, sourcePtr, mapSource.RowPitch * height);
            bmp.UnlockBits(bmpData);
            _device.ImmediateContext.UnmapSubresource(screenTexture, 0);
            duplicatedOutput.ReleaseFrame();
        }
        catch (SharpDXException ex)
        {
            Log.Error(ex, "Exception occurred while capturing screen");
        }
        finally
        {
            screenResource?.Dispose();
        }

        var imageBytes = ImageToBytes(bmp);
        operation.Complete();
        return imageBytes;
	}

	public Task<byte[]> CaptureAsync(CancellationToken cancellationToken = default) =>
		Task.Run(Capture, cancellationToken);

	public Game? Game { get; set; }
	public ushort? Resolution { get; set; }
	public ushort XOffset { get; set; }
	public ushort YOffset { get; set; }
	public bool CanCapture => true;
	
	private static byte[] ImageToBytes(Image bitmap)
	{
		using MemoryStream stream = new();
		bitmap.Save(stream, ImageFormat.Bmp);
		return stream.ToArray();
	}
}