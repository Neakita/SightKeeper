using CommunityToolkit.Diagnostics;
using SightKeeper.Domain;
using SixLabors.ImageSharp;

namespace SightKeeper.Application.Training.Data;

public sealed class ImageDataValue : ImageData
{
	public Vector2<ushort> Size => _domainImage.Size;
	public DateTimeOffset CreationTimestamp => _domainImage.CreationTimestamp;

	public Image Image
	{
		get
		{
			using var stream = _domainImage.OpenReadStream();
			Guard.IsNotNull(stream);
			return Image.Load(stream);
		}
	}

	public ImageDataValue(Domain.Images.Image domainImage)
	{
		_domainImage = domainImage;
	}

	private readonly Domain.Images.Image _domainImage;
}