namespace SightKeeper.Domain.Model;

public abstract class WeightsData
{
    public byte[] Content { get; private set; }

    protected WeightsData(byte[] content)
    {
        Content = content;
    }
}