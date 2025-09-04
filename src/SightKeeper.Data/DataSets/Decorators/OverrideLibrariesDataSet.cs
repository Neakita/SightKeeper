using SightKeeper.Domain;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.DataSets.Weights;

namespace SightKeeper.Data.DataSets.Decorators;

internal sealed class OverrideLibrariesDataSet<TAsset>(DataSet<TAsset> inner) : DataSet<TAsset>, Decorator<DataSet<TAsset>>
{
	public string Name
	{
		get => inner.Name;
		set => inner.Name = value;
	}

	public string Description
	{
		get => inner.Description;
		set => inner.Description = value;
	}

	public TagsOwner<Tag> TagsLibrary { get; init; } = inner.TagsLibrary;
	public AssetsOwner<TAsset> AssetsLibrary { get; init; } = inner.AssetsLibrary;
	public WeightsLibrary WeightsLibrary { get; init; } = inner.WeightsLibrary;
	public DataSet<TAsset> Inner => inner;
}