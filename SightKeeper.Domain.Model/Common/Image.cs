namespace SightKeeper.Domain.Model.Common;

public sealed class Image
{
	internal Screenshot? Screenshot { get; set; }
	public byte[] Content { get; private set; }
	
	public Image(byte[] content)
	{
		Content = content;
	}
}