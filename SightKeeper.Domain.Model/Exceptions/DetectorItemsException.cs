using SightKeeper.Domain.Model.DataSets.Screenshots;

namespace SightKeeper.Domain.Model.Exceptions;

public sealed class DetectorItemsException : Exception
{
    public IReadOnlyCollection<DetectorItem> Items { get; }

    internal DetectorItemsException(IEnumerable<DetectorItem> items, string message) : base(message)
    {
        Items = items.ToList();
    }
}