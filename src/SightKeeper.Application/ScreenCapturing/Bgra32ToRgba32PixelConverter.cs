using System.Numerics.Tensors;
using System.Runtime.InteropServices;
using Serilog;
using Serilog.Events;
using SerilogTimings.Extensions;
using SixLabors.ImageSharp.PixelFormats;

namespace SightKeeper.Application.ScreenCapturing;

internal sealed class Bgra32ToRgba32PixelConverter : PixelConverter<Bgra32, Rgba32>
{
	public override void Convert(ReadOnlySpan<Bgra32> source, Span<Rgba32> target)
	{
		using var operation = Logger.BeginOperation("Converting {count} pixels", source.Length);
		var packedSource = MemoryMarshal.Cast<Bgra32, uint>(source);
		var packedTarget = MemoryMarshal.Cast<Rgba32, uint>(target);
		// with first call we should erase previous data that can come from ArrayPool
		CopyChannel(packedSource, 0, packedTarget, 16);
		MergeChannel(packedSource, 8, packedTarget, 8);
		MergeChannel(packedSource, 16, packedTarget, 0);
		TensorPrimitives.Add(packedTarget, ChannelMask << 24, packedTarget);
		operation.Complete(LogEventLevel.Verbose);
	}

	private static readonly ILogger Logger = Log.ForContext<Bgra32ToRgba32PixelConverter>();
	private const uint ChannelMask = 0xFF;
	private static uint[] _buffer = Array.Empty<uint>();

	private static void CopyChannel(ReadOnlySpan<uint> source, int sourceChannelShift, Span<uint> target, int targetChannelShift)
	{
		var relativeShift = sourceChannelShift - targetChannelShift;
		TensorPrimitives.BitwiseAnd(source, ChannelMask << sourceChannelShift, target);
		if (relativeShift > 0)
			TensorPrimitives.ShiftRightLogical(target, relativeShift, target);
		else if (relativeShift < 0)
			TensorPrimitives.ShiftLeft(target, -relativeShift, target);
	}

	private static void MergeChannel(ReadOnlySpan<uint> source, int sourceShift, Span<uint> target, int targetShift)
	{
		var relativeShift = sourceShift - targetShift;
		EnsureBufferCapacity(source.Length);
		Span<uint> buffer = _buffer.AsSpan(0, source.Length);
		TensorPrimitives.BitwiseAnd(source, ChannelMask << sourceShift, buffer);
		if (relativeShift > 0)
			TensorPrimitives.ShiftRightLogical(buffer, relativeShift, buffer);
		else if (relativeShift < 0)
			TensorPrimitives.ShiftLeft(buffer, -relativeShift, buffer);
		TensorPrimitives.BitwiseOr(target, buffer, target);
	}

	private static void EnsureBufferCapacity(int capacity)
	{
		if (_buffer.Length < capacity)
			_buffer = new uint[capacity];
	}
}