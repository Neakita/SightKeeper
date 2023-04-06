using ReactiveUI;
using SightKeeper.Domain.Model.Abstract;

namespace SightKeeper.Domain.Model.Common;

public sealed class Image : ReactiveObject, Entity
{
	public Image(byte[] content)
	{
		Content = content;
	}

	private Image(int id, byte[] content)
	{
		Id = id;
		Content = content;
	}
	
	public int Id { get; private set; }
	public byte[] Content { get; private set; }
}