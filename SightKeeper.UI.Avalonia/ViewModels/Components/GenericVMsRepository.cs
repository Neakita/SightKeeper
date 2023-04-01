using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using DynamicData;
using DynamicData.Kernel;
using SightKeeper.Domain.Model.Abstract;
using SightKeeper.Domain.Services;
using SightKeeper.Infrastructure.Common;
using SightKeeper.UI.Avalonia.ViewModels.Elements;

namespace SightKeeper.UI.Avalonia.ViewModels.Components;

public sealed class GenericVMsRepository<TItemVM, TItem> : Repository<TItemVM> where TItemVM : ItemVM<TItem> where TItem : Entity
{
	public ReadOnlyObservableCollection<TItemVM> Items { get; }

	public ISourceCache<TItemVM, int> ItemsCache { get; } =
		new SourceCache<TItemVM, int>(modelVM => modelVM.Item.Id);
	

	public GenericVMsRepository(DynamicRepository<TItem> repository)
	{
		_repository = repository;
		
		repository.ItemsCache
			.Connect()
			.Transform(Locator.Resolve<TItemVM, TItem>)
			.PopulateInto(ItemsCache);

		ItemsCache.Connect().Bind(out ReadOnlyObservableCollection<TItemVM> items).Subscribe();
		
		Items = items;
	}

	public TItemVM Get(int id) => ItemsCache.Lookup(id)
		.ValueOrThrow(() => new Exception($"Not found {nameof(ModelVM<Model>)} with id {id}"));

	public bool Contains(TItemVM itemVM)
	{
		Optional<TItemVM> lookup = ItemsCache.Lookup(itemVM.Item.Id);
		return lookup.HasValue && IsEqual(lookup.Value, itemVM);
	}
	
	private static bool IsEqual<T>(T x, T y) => EqualityComparer<T>.Default.Equals(x, y);


	public void Add(TItemVM item)
	{
		_repository.Add(item.Item);
		ItemsCache.AddOrUpdate(item);
	}

	public void Remove(TItemVM item)
	{
		_repository.Remove(item.Item);
	}
	
	private readonly Repository<TItem> _repository;
}
