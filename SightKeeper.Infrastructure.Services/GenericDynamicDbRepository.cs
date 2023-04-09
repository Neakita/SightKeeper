using System.Collections.ObjectModel;
using DynamicData;
using DynamicData.Kernel;
using Microsoft.EntityFrameworkCore;
using SightKeeper.Domain.Model.Abstract;
using SightKeeper.Domain.Services;
using SightKeeper.Infrastructure.Data;

namespace SightKeeper.Infrastructure.Services;

public class GenericDynamicDbRepository<TEntity> : DynamicRepository<TEntity> where TEntity : class, Entity
{
	public ReadOnlyObservableCollection<TEntity> Items { get; }
	public ISourceCache<TEntity, int> ItemsCache { get; }

	public GenericDynamicDbRepository(AppDbContextFactory dbContextFactory)
	{
		DbContextFactory = dbContextFactory;

		ItemsCache = new SourceCache<TEntity, int>(entity => entity.Id);
		ItemsCache
			.Connect()
			.Bind(out ReadOnlyObservableCollection<TEntity> items)
			.Subscribe();
		Items = items;
		
		using AppDbContext dbContext = dbContextFactory.CreateDbContext();
		ItemsCache.AddOrUpdate(dbContext.Set<TEntity>());
	}

	public TEntity Get(int id) =>
		ItemsCache.Lookup(id)
			.ValueOrThrow(() => new Exception($"Item of type {typeof(TEntity)} with id {id} not found"));

	public bool Contains(TEntity itemVM) => ItemsCache.Lookup(itemVM.Id).HasValue;

	public void Add(TEntity item)
	{
		using AppDbContext dbContext = DbContextFactory.CreateDbContext();
		DbSet<TEntity> set = dbContext.Set<TEntity>();
		set.Add(item);
		dbContext.SaveChanges();
		ItemsCache.AddOrUpdate(item);
	}

	public void Remove(TEntity item)
	{
		
		using AppDbContext dbContext = DbContextFactory.CreateDbContext();
		DbSet<TEntity> set = dbContext.Set<TEntity>();
		set.Remove(item);
		dbContext.SaveChanges();
		ItemsCache.Remove(item);
	}
	
	protected readonly AppDbContextFactory DbContextFactory;
}
