using SightKeeper.Domain.Model.DataSets.Screenshots.Assets.Detector;

namespace SightKeeper.Domain.Model.Exceptions;

public sealed class DetectorItemException : Exception
{
    public DetectorItem Item { get; }

    internal DetectorItemException(DetectorItem item, string message) : base(message)
    {
        Item = item;
    }
}