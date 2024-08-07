namespace SightKeeper.Domain.Model.DataSets.Weights;

public sealed class WeightsData
{
    public byte[] Content { get; }

    internal WeightsData(byte[] content)
    {
        Content = content;
    }
}