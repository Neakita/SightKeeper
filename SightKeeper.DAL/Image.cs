using SightKeeper.DAL.Domain.Common;

namespace SightKeeper.DAL;

public sealed class Image
{
	public Image(byte[] content, Resolution resolution)
	{
		CreationDate = DateTime.UtcNow;
		Content = content;
		Resolution = resolution;
	}

	private Image(int id, byte[] content, DateTime creationDate)
	{
		Id = id;
		Content = content;
		Resolution = null!;
		CreationDate = creationDate;
	}
	
	public int Id { get; private set; }
	public byte[] Content { get; private set; }
	public Resolution Resolution { get; private set; }
	public DateTime CreationDate { get; private set; }
}
