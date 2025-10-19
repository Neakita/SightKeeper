namespace SightKeeper.Data.ImageSets.Images;

public interface StreamableData
{
	Stream OpenWriteStream();
	Stream OpenReadStream();
}