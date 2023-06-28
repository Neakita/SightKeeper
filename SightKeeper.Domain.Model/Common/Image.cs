namespace SightKeeper.Domain.Model.Common;

public sealed class Image
{
	public Image(byte[] content)
	{
		Content = content;
	}
	
	public byte[] Content { get; private set; }
}