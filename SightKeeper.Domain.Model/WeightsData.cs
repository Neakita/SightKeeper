namespace SightKeeper.Domain.Model;

public sealed class WeightsData : Entity
{
    public byte[] Content { get; }

    internal WeightsData(byte[] content)
    {
        Content = content;
    }
}