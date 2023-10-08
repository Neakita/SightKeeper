using FlakeId;

namespace SightKeeper.Domain.Model;

public abstract class WeightsData
{
    public Id Id { get; private set; }
    public byte[] Content { get; private set; }

    protected WeightsData(byte[] content)
    {
        Content = content;
    }
}