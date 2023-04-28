using System.Drawing;
using System.Drawing.Imaging;
using SharpDX;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using SightKeeper.Application;
using SightKeeper.Domain.Model.Common;
using Image = System.Drawing.Image;

namespace SightKeeper.Services.Windows;

public sealed class DirectXScreenCapture : ScreenCapture
{
	public byte[] Capture()
	{
		throw new NotSupportedException();
		var factory = new Factory1();
        var adapter = factory.GetAdapter1(0);
        Console.WriteLine(adapter.Description1.Description);
        var device = new SharpDX.Direct3D11.Device(adapter);
        Output output = adapter.GetOutput(0);
        Console.WriteLine(output.Description.DeviceName);
        Output1 output1 = output.QueryInterface<Output1>();

        int width = output.Description.DesktopBounds.Right;
        int height = output.Description.DesktopBounds.Bottom;

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
        using Texture2D screenTexture = new(device, textureDesc);
        using OutputDuplication duplicatedOutput = output1.DuplicateOutput(device);
        Thread.Sleep(20); // захватчику экрана надо время проинициализироваться
        Bitmap bmp = new(width, height, PixelFormat.Format32bppArgb);
        SharpDX.DXGI.Resource screenResource = null;
        try
        {
            if (duplicatedOutput.TryAcquireNextFrame(10, out OutputDuplicateFrameInformation _, out screenResource) != Result.Ok)
                return ImageToBytes(bmp);

            using (Texture2D screenTexture2D = screenResource.QueryInterface<Texture2D>())
            {
                device.ImmediateContext.CopyResource(screenTexture2D, screenTexture);
            }

            DataBox mapSource = device.ImmediateContext.MapSubresource(screenTexture, 0, MapMode.Read, SharpDX.Direct3D11.MapFlags.None);
            BitmapData bmpData = bmp.LockBits(new Rectangle(Point.Empty, bmp.Size), ImageLockMode.WriteOnly, bmp.PixelFormat);
            nint sourcePtr = mapSource.DataPointer;
            nint destPtr = bmpData.Scan0;
            Utilities.CopyMemory(destPtr, sourcePtr, mapSource.RowPitch * height);
            bmp.UnlockBits(bmpData);
            device.ImmediateContext.UnmapSubresource(screenTexture, 0);
            duplicatedOutput.ReleaseFrame();
        }
        catch (SharpDXException ex)
        {
            Console.WriteLine(ex.Message);
        }
        finally
        {
            screenResource?.Dispose();
        }
        return ImageToBytes(bmp);
	}

	public Game? Game { get; set; }
	public Resolution? Resolution { get; set; }
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