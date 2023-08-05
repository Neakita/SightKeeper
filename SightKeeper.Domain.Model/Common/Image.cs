namespace SightKeeper.Domain.Model.Common;

public abstract class Image
{
    public byte[] Content { get; private set; }

    protected Image(byte[] content)
    {
        Content = content;
    }
}