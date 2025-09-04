using CommunityToolkit.Diagnostics;
using SightKeeper.Domain;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Classifier;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.DataSets.Classifier.Assets.Decorators;

internal sealed class TagUsersTrackingClassifierAsset(ClassifierAsset inner) : ClassifierAsset, Decorator<ClassifierAsset>
{
	public ManagedImage Image => inner.Image;

	public AssetUsage Usage
	{
		get => inner.Usage;
		set => inner.Usage = value;
	}
	
	public TagUser? TagUser { get; set; }

	public Tag Tag
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

	public ClassifierAsset Inner => inner;
}