using CommunityToolkit.Diagnostics;
using SightKeeper.Application.Misc;
using SightKeeper.Data.DataSets.Poser.Items.KeyPoints;
using SightKeeper.Data.Services;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Poser;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Data.DataSets.Poser.Items;

internal sealed class StorablePoserItemFactory(
	ChangeListener changeListener,
	Lock editingLock,
	KeyPointFactory keyPointFactory)
	: Factory<PoserItem>, PostWrappingInitializable<DataSet<PoserTag, ReadOnlyAsset>>
{
	public void Initialize(DataSet<PoserTag, ReadOnlyAsset> wrapped)
	{
		_tagsContainer = wrapped.TagsLibrary;
	}

	public PoserItem Create()
	{
		Guard.IsNotNull(_tagsContainer);
		var tag = _tagsContainer.Tags[0];
		return new InMemoryPoserItem(tag, keyPointFactory)
			.WithTracking(changeListener)
			.WithLocking(editingLock)
			.WithObservableKeyPoints()
			.WithDomainRules()
			.WithNotifications();
	}

	private TagsContainer<PoserTag>? _tagsContainer;
}