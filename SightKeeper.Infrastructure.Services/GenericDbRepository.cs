using Microsoft.EntityFrameworkCore;
using SightKeeper.Domain.Model.Abstract;
using SightKeeper.Domain.Services;
using SightKeeper.Infrastructure.Data;

namespace SightKeeper.Infrastructure.Services;

public sealed class GenericDbRepository<TItem> : Repository<TItem> where TItem : class, Entity
{
	public IReadOnlyCollection<TItem> Items => _items;
	public TItem Get(int id) => _itemsByIds[id];
	public bool Contains(TItem item) => _itemsByIds.ContainsKey(item.Id);

	public GenericDbRepository(AppDbContextFactory dbContextFactory)
	{
		_dbContextFactory = dbContextFactory;
		using AppDbContext dbContext = dbContextFactory.CreateDbContext();
		DbSet<TItem> set = dbContext.Set<TItem>();
		_items = set.ToList();
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
	private readonly List<TItem> _items;
	private readonly Dictionary<int, TItem> _itemsByIds;
}
