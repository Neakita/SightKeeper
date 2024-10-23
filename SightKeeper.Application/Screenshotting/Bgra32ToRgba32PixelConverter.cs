using System.Numerics.Tensors;
using System.Runtime.InteropServices;
using SixLabors.ImageSharp.PixelFormats;

namespace SightKeeper.Application.Screenshotting;

public class Bgra32ToRgba32PixelConverter : PixelConverter<Bgra32, Rgba32>
{
	protected override void Convert(ReadOnlySpan<Bgra32> source, Span<Rgba32> target)
	{
		var packedSource = MemoryMarshal.Cast<Bgra32, uint>(source);
		var packedTarget = MemoryMarshal.Cast<Rgba32, uint>(target);
		CopyChannel(packedSource, 0, packedTarget, 16);
		CopyChannel(packedSource, 8, packedTarget, 8);
		CopyChannel(packedSource, 16, packedTarget, 0);
		TensorPrimitives.Add(packedTarget, ChannelMask << 24, packedTarget);
	}

	private const uint ChannelMask = 0xFF;

	private static void CopyChannel(ReadOnlySpan<uint> source, int sourceChannelShift, Span<uint> target, int targetChannelShift)
	{
		var relativeShift = sourceChannelShift - targetChannelShift;
		Span<uint> buffer = stackalloc uint[source.Length];
		TensorPrimitives.BitwiseAnd(source, ChannelMask << sourceChannelShift, buffer);
		if (relativeShift > 0)
			TensorPrimitives.ShiftRightArithmetic(buffer, relativeShift, buffer);
		else if (relativeShift < 0)
			TensorPrimitives.ShiftLeft(buffer, -relativeShift, buffer);
		TensorPrimitives.BitwiseOr(target, buffer, target);
	}
}