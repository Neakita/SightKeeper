namespace SightKeeper.Domain.Model.Common;

public sealed class Screenshot
{
	public Image Image { get; private set; }
	public DateTime CreationDate { get; private set; }
	
	public Screenshot(Image image)
	{
		Image = image;
		CreationDate = DateTime.UtcNow;
	}

	private Screenshot()
	{
		Image = null!;
	}
}