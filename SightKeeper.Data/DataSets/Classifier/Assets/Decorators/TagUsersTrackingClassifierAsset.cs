using CommunityToolkit.Diagnostics;
using SightKeeper.Data.DataSets.Tags;
using SightKeeper.Data.ImageSets.Images;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Data.DataSets.Classifier.Assets.Decorators;

internal sealed class TagUsersTrackingClassifierAsset(StorableClassifierAsset inner) : StorableClassifierAsset
{
	public StorableImage Image => inner.Image;

	public AssetUsage Usage
	{
		get => inner.Usage;
		set => inner.Usage = value;
	}
	
	public TagUser? TagUser { get; set; }

	public StorableTag Tag
	{
		get => inner.Tag;
		set
		{
			if (Tag == value)
				return;
			Guard.IsNotNull(TagUser);
			Tag.RemoveUser(TagUser);
			inner.Tag = value;
			Tag.AddUser(TagUser);
		}
	}
}