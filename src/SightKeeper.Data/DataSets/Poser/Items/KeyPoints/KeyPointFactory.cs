using SightKeeper.Domain.DataSets.Poser;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Data.DataSets.Poser.Items.KeyPoints;

internal interface KeyPointFactory
{
	KeyPoint CreateKeyPoint(Tag tag);
}