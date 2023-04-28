using System.Collections.ObjectModel;
using DynamicData;
using SightKeeper.Domain.Services;

namespace SightKeeper.Services;

public sealed class InheritedGenericRepository<TItem, TBaseItem> : Repository<TItem> where TItem : TBaseItem
{
	public ReadOnlyObservableCollection<TItem> Items { get; }
	public TItem Get(int id) => throw new NotImplementedException();

	public bool Contains(TItem itemVM) => throw new NotImplementedException();
	
	public InheritedGenericRepository(DynamicRepository<TBaseItem> baseRepository)
	{
		_baseRepository = baseRepository;
		baseRepository.ItemsCache
			.Connect()
			.Filter(item => item.GetType() == typeof(TItem))
			.Cast(baseItem => (TItem)baseItem)
			.Bind(out ReadOnlyObservableCollection<TItem> items)
			.Subscribe();
		Items = items;
	}

	public void Add(TItem item) => _baseRepository.Add(item);

	public void Remove(TItem item) => _baseRepository.Remove(item);

	private readonly DynamicRepository<TBaseItem> _baseRepository;
}
