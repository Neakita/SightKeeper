using SightKeeper.Domain.DataSets.Poser;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Data.DataSets.Poser.Items.KeyPoints;

internal sealed class WrappingKeyPointFactory(KeyPointFactory inner, KeyPointWrapper wrapper) : KeyPointFactory
{
	public KeyPoint CreateKeyPoint(Tag tag)
	{
		var keyPoint = inner.CreateKeyPoint(tag);
		return wrapper.Wrap(keyPoint);
	}
}