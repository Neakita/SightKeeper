using System;
using System.Collections.Generic;
using Vibrance.Changes;

namespace SightKeeper.Avalonia.Extensions;

internal static class ObservableChangesExtensions
{
	public static IDisposable ToDictionary<TValue, TKey>(
		this IObservable<Change<TValue>> changes,
		Func<TValue, TKey> keySelector,
		out Dictionary<TKey, TValue> result)
		where TKey : notnull
	{
		Dictionary<TKey, TValue> dictionary = new();
		result = dictionary;
		return changes.Subscribe(change =>
		{
			if (change is Move<TValue>)
				return;
			foreach (var oldItem in change.OldItems)
			{
				var key = keySelector(oldItem);
				dictionary.Remove(key);
			}
			foreach (var newItem in change.NewItems)
			{
				var key = keySelector(newItem);
				dictionary.Add(key, newItem);
			}
		});
	}
}