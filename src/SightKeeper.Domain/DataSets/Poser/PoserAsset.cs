using SightKeeper.Domain.DataSets.Assets.Items;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Domain.DataSets.Poser;

public interface PoserAsset : ItemsAsset<PoserItem>
{
	PoserItem MakeItem(PoserTag tag);

	PoserItem ItemsMaker<PoserItem>.MakeItem(Tag tag)
	{
		return MakeItem((PoserTag)tag);
	}
}