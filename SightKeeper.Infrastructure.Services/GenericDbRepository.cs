using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore;
using SightKeeper.Domain.Model.Abstract;
using SightKeeper.Domain.Services;
using SightKeeper.Infrastructure.Data;

namespace SightKeeper.Infrastructure.Services;

public sealed class GenericDbRepository<TItem> : Repository<TItem> where TItem : class, Entity
{
	public ReadOnlyObservableCollection<TItem> Items { get; }
	public TItem Get(int id) => _itemsByIds[id];
	public bool Contains(TItem itemVM) => _itemsByIds.ContainsKey(itemVM.Id);

	public GenericDbRepository(AppDbContextFactory dbContextFactory)
	{
		_dbContextFactory = dbContextFactory;
		using AppDbContext dbContext = dbContextFactory.CreateDbContext();
		DbSet<TItem> set = dbContext.Set<TItem>();
		_items = new ObservableCollection<TItem>(set);
		Items = new ReadOnlyObservableCollection<TItem>(_items);
		_itemsByIds = _items.ToDictionary(item => item.Id);
	}

	public void Add(TItem item)
	{
		using AppDbContext dbContext = _dbContextFactory.CreateDbContext();
		DbSet<TItem> set = dbContext.Set<TItem>();
		set.Add(item);
		dbContext.SaveChanges();
		_items.Add(item);
		_itemsByIds.Add(item.Id, item);
	}

	public void Remove(TItem item)
	{
		using AppDbContext dbContext = _dbContextFactory.CreateDbContext();
		DbSet<TItem> set = dbContext.Set<TItem>();
		set.Remove(item);
		dbContext.SaveChanges();
		_items.Remove(item);
		_itemsByIds.Remove(item.Id);
	}
	
	private readonly AppDbContextFactory _dbContextFactory;
	private readonly ObservableCollection<TItem> _items;
	private readonly Dictionary<int, TItem> _itemsByIds;
}
