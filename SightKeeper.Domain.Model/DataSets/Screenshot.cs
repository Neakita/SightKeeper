namespace SightKeeper.Domain.Model.DataSets;

public sealed class Screenshot
{
	public Image Image { get; }
	public DateTime CreationDate { get; }
	public Asset? Asset { get; internal set; }

	internal Screenshot(byte[] content)
	{
		Image = new Image(content);
		CreationDate = DateTime.Now;
	}

	private Screenshot()
	{
		Image = null!;
	}
}