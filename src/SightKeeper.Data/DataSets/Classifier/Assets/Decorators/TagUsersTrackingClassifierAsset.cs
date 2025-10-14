using CommunityToolkit.Diagnostics;
using SightKeeper.Data.DataSets.Tags;
using SightKeeper.Domain;
using SightKeeper.Domain.DataSets.Assets;
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
			Tag.Get<EditableTagUsers>().RemoveUser(TagUser);
			inner.Tag = value;
			Tag.Get<EditableTagUsers>().AddUser(TagUser);
		}
	}

	public ClassifierAsset Inner => inner;
}