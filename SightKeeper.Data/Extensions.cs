using FlakeId;
using SightKeeper.Data.DataSets.Classifier;
using SightKeeper.Data.DataSets.Weights;
using SightKeeper.Domain.DataSets.Classifier;
using SightKeeper.Domain.DataSets.Weights;

namespace SightKeeper.Data;

internal static class Extensions
{
	public static TTarget UnWrapDecorator<TTarget>(this object source)
	{
		if (source is TTarget target)
			return target;
		if (source is Decorator<object> decorator)
			return UnWrapDecorator<TTarget>(decorator.Inner);
		throw new ArgumentException($"Provided object of type {source.GetType()} could not be unwrapped to {typeof(TTarget)}");
	}

	public static StorableClassifierDataSet WithTracking(this StorableClassifierDataSet set, ChangeListener listener)
	{
		return new TrackableClassifierDataSet(set, listener);
	}

	public static StorableClassifierDataSet WithLocking(this StorableClassifierDataSet set, Lock editingLock)
	{
		return new LockingClassifierDataSet(set, editingLock);
	}

	public static StorableClassifierDataSet WithDomainRules(this StorableClassifierDataSet set)
	{
		return new StorableClassifierDataSetExtension(new DomainClassifierDataSet(set), set);
	}

	public static Id GetId(this Weights weights)
	{
		var inMemoryWeights = weights.UnWrapDecorator<InMemoryWeights>();
		return inMemoryWeights.Id;
	}
}