namespace SightKeeper.Domain.Model.DataSets;

public sealed class Image
{
    public byte[] Content { get; }

    internal Image(byte[] content)
    {
        Content = content;
    }
}