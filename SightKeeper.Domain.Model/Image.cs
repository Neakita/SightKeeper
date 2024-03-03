namespace SightKeeper.Domain.Model;

public sealed class Image : Entity
{
    public byte[] Content { get; }

    internal Image(byte[] content)
    {
        Content = content;
    }
}