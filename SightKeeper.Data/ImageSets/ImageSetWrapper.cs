namespace SightKeeper.Data.ImageSets;

internal interface ImageSetWrapper
{
	StorableImageSet Wrap(StorableImageSet set);
}