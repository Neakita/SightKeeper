using SightKeeper.DAL.Domain.Common;

namespace SightKeeper.DAL;

public sealed class Image
{
	public Image(byte[] content, Resolution resolution)
	{
		Content = content;
		Resolution = resolution;
	}

	private Image(int id, byte[] content)
	{
		Id = id;
		Content = content;
		Resolution = null!;
	}
	
	public int Id { get; private set; }
	public byte[] Content { get; private set; }
	public Resolution Resolution { get; private set; }
}