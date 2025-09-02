using System;
using System.Numerics;
using Pure.DI;
using SixLabors.ImageSharp.PixelFormats;

namespace SightKeeper.Avalonia.Compositions;

[GenericTypeArgument]
// ReSharper disable InconsistentNaming
public readonly struct TTPixel : IPixel<TTPixel>
// ReSharper restore InconsistentNaming
{
	public PixelOperations<TTPixel> CreatePixelOperations()
	{
		throw new NotSupportedException();
	}

	public void FromScaledVector4(Vector4 vector)
	{
		throw new NotSupportedException();
	}

	public Vector4 ToScaledVector4()
	{
		throw new NotSupportedException();
	}

	public void FromVector4(Vector4 vector)
	{
		throw new NotSupportedException();
	}

	public Vector4 ToVector4()
	{
		throw new NotSupportedException();
	}

	public void FromArgb32(Argb32 source)
	{
		throw new NotSupportedException();
	}

	public void FromBgra5551(Bgra5551 source)
	{
		throw new NotSupportedException();
	}

	public void FromBgr24(Bgr24 source)
	{
		throw new NotSupportedException();
	}

	public void FromBgra32(Bgra32 source)
	{
		throw new NotSupportedException();
	}

	public void FromAbgr32(Abgr32 source)
	{
		throw new NotSupportedException();
	}

	public void FromL8(L8 source)
	{
		throw new NotSupportedException();
	}

	public void FromL16(L16 source)
	{
		throw new NotSupportedException();
	}

	public void FromLa16(La16 source)
	{
		throw new NotSupportedException();
	}

	public void FromLa32(La32 source)
	{
		throw new NotSupportedException();
	}

	public void FromRgb24(Rgb24 source)
	{
		throw new NotSupportedException();
	}

	public void FromRgba32(Rgba32 source)
	{
		throw new NotSupportedException();
	}

	public void ToRgba32(ref Rgba32 dest)
	{
		throw new NotSupportedException();
	}

	public void FromRgb48(Rgb48 source)
	{
		throw new NotSupportedException();
	}

	public void FromRgba64(Rgba64 source)
	{
		throw new NotSupportedException();
	}

	public bool Equals(TTPixel other)
	{
		throw new NotSupportedException();
	}
}