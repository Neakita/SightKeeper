using ReactiveUI;
using SightKeeper.Domain.Model.Abstract;

namespace SightKeeper.Domain.Model.Common;

public sealed class Image : ReactiveObject, Entity
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