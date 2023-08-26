namespace SightKeeper.Domain.Model.Common;

public sealed class Image
{
    public byte[] Content { get; private set; }

    public Image(byte[] content)
    {
        Content = content;
    }
}