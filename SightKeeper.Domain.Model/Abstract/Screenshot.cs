using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Domain.Model.Abstract;

public sealed class Screenshot : Entity
{
	public Image Image { get; private set; }
	public DateTime CreationDate { get; private set; }
	
	public Screenshot(Image image)
	{
		Image = image;
		CreationDate = DateTime.UtcNow;
	}

	private Screenshot(int id) : base(id)
	{
		Image = null!;
	}
}