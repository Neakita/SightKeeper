using System.ComponentModel.DataAnnotations.Schema;
using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Domain.Model.Abstract;

public abstract class Screenshot : Entity
{
	[ForeignKey(nameof(Id))]
	public Image Image { get; private set; }
	public Game? Game { get; private set; }
	public DateTime CreationDate { get; private set; }
	
	public Screenshot(Image image, Game? game = null)
	{
		Image = image;
		Game = game;
		CreationDate = DateTime.UtcNow;
	}
	
	protected Screenshot(int id) : base(id)
	{
		Image = null!;
	}
}