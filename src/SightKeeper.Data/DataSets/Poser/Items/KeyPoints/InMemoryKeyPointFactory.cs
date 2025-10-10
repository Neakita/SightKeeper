using SightKeeper.Domain.DataSets.Poser;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Data.DataSets.Poser.Items.KeyPoints;

internal sealed class InMemoryKeyPointFactory : KeyPointFactory
{
	public KeyPoint CreateKeyPoint(Tag tag)
	{
		return new InMemoryKeyPoint
		{
			Tag = tag
		};
	}
}