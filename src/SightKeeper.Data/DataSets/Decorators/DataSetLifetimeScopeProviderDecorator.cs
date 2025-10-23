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
	ILifetimeScope lifetimeScope,
	Action<ContainerBuilder>? configurationAction) :
	DataSet<TTag, TAsset>,
	Decorator<DataSet<TTag, TAsset>>,
	LifetimeScopeProviderDecorator,
	PostWrappingInitializable<DataSet<TTag, TAsset>>,
	IDisposable,
	IAsyncDisposable
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
	public ILifetimeScope LifetimeScope
	{
		get
		{
			Guard.IsNotNull(_lifetimeScope);
			return _lifetimeScope;
		}
	}

	public void Initialize(DataSet<TTag, TAsset> wrapped)
	{
		_lifetimeScope = lifetimeScope.BeginLifetimeScope(builder =>
		{
			builder.RegisterInstance(wrapped);
			configurationAction?.Invoke(builder);
		});
	}

	public void Dispose()
	{
		Guard.IsNotNull(_lifetimeScope);
		_lifetimeScope.Dispose();
	}

	public async ValueTask DisposeAsync()
	{
		Guard.IsNotNull(_lifetimeScope);
		await _lifetimeScope.DisposeAsync();
	}

	private ILifetimeScope? _lifetimeScope;
}