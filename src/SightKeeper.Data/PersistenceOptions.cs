using Autofac;

namespace SightKeeper.Data;

public sealed class PersistenceOptions
{
	public Action<ContainerBuilder>? ClassifierDataSetScopeConfiguration { get; init; }
	public Action<ContainerBuilder>? DetectorDataSetScopeConfiguration { get; init; }
	public Action<ContainerBuilder>? PoserDataSetScopeConfiguration { get; init; }
}