﻿using System.Collections.ObjectModel;

namespace SightKeeper.Domain.Services;

public interface Repository<TItem>
{
	ReadOnlyObservableCollection<TItem> Items { get; }

	TItem Get(int id);
	bool Contains(TItem modelVM);
	void Add(TItem item);
	void Remove(TItem item);
}
