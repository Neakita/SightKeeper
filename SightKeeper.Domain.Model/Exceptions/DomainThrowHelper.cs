using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using SightKeeper.Domain.Model.DataSet.Screenshots.Assets.Detector;

namespace SightKeeper.Domain.Model.Exceptions;

internal static class DomainThrowHelper
{
    [DoesNotReturn]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ThrowDetectorItemException(DetectorItem item, string message)
    {
        throw new DetectorItemException(item, message);
    }

    [DoesNotReturn]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ThrowDetectorItemsException(IEnumerable<DetectorItem> items, string message)
    {
        throw new DetectorItemsException(items, message);
    }
}