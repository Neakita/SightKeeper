namespace SightKeeper.Domain.Model.Common;

public abstract class Image
{
    public byte[] Content { get; private set; }
    public Resolution Resolution { get; private set; }

    protected Image(byte[] content, Resolution resolution)
    {
        Content = content;
        Resolution = resolution;
    }
}