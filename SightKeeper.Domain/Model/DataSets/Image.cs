namespace SightKeeper.Domain.Model.DataSets;

public sealed class Image
{
    public byte[] Data { get; }

    internal Image(byte[] data)
    {
        Data = data;
    }
}