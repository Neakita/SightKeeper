using Autofac;
using CommunityToolkit.Diagnostics;
using SightKeeper.Data.Services;
using SightKeeper.Domain;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.DataSets.Weights;

namespace SightKeeper.Data.DataSets.Decorators;

internal sealed class DataSetLifetimeScopeProviderDecorator<TTag, TAsset>(
	DataSet<TTag, TAsset> inner,
	Action<ContainerBuilder>? configurationAction) :
	DataSet<TTag, TAsset>,
	Decorator<DataSet<TTag, TAsset>>,
	LifetimeScopeProviderDecorator,
	PostWrappingInitializable<DataSet<TTag, TAsset>>
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

	public TagsOwner<TTag> TagsLibrary => inner.TagsLibrary;
	public AssetsOwner<TAsset> AssetsLibrary => inner.AssetsLibrary;
	public WeightsLibrary WeightsLibrary => inner.WeightsLibrary;
	public DataSet<TTag, TAsset> Inner => inner;

	public void Initialize(DataSet<TTag, TAsset> wrapped)
	{
		_wrapped = wrapped;
	}

	public ILifetimeScope BeginLifetimeScope(ILifetimeScope scope)
	{
		Guard.IsNotNull(_wrapped);
		return scope.BeginLifetimeScope(typeof(DataSet<TTag, TAsset>), builder =>
		{
			builder.RegisterInstance(_wrapped);
			configurationAction?.Invoke(builder);
		});
	}

	private DataSet<TTag, TAsset>? _wrapped;
}