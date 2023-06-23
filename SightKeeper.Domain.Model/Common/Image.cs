using SightKeeper.Domain.Model.Abstract;

namespace SightKeeper.Domain.Model.Common;

public sealed class Image : Entity
{
	public Image(byte[] content)
	{
		Content = content;
	}

	private Image(int id, byte[] content) : base(id)
	{
		Content = content;
	}
	
	public byte[] Content { get; private set; }
}