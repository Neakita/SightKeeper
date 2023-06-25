using System.ComponentModel.DataAnnotations.Schema;
using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Domain.Model.Abstract;

public sealed class Screenshot : Entity
{
	[ForeignKey(nameof(Id))]
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