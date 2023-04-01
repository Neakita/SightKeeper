using ReactiveUI;
using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Domain.Model.Abstract;

public abstract class Screenshot : ReactiveObject, Entity
{
	public Screenshot(Image image)
	{
		Image = image;
		CreationDate = DateTime.UtcNow;
	}


	protected Screenshot(int id)
	{
		Id = id;
		Image = null!;
	}

	public int Id { get; private set; }
	public Image Image { get; private set; }
	public DateTime CreationDate { get; private set; }
}