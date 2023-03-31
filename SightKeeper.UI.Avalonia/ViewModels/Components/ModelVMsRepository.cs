using System;
using System.Collections.ObjectModel;
using DynamicData;
using DynamicData.Kernel;
using SightKeeper.Domain.Model.Abstract;
using SightKeeper.Domain.Services;
using SightKeeper.UI.Avalonia.ViewModels.Elements;

namespace SightKeeper.UI.Avalonia.ViewModels.Components;

public sealed class ModelVMsRepository : DynamicRepository<ModelVM>
{
	public ReadOnlyObservableCollection<ModelVM> Items { get; }

	public ISourceCache<ModelVM, int> ItemsCache { get; } =
		new SourceCache<ModelVM, int>(modelVM => modelVM.Model.Id);
	

	public ModelVMsRepository(DynamicRepository<Model> modelsRepository)
	{
		_modelsRepository = modelsRepository;
		
		modelsRepository.ItemsCache
			.Connect()
			.Transform(ModelVM.Create)
			.PopulateInto(ItemsCache);

		ItemsCache.Connect().Bind(out ReadOnlyObservableCollection<ModelVM> items).Subscribe();
		
		Items = items;
	}

	public ModelVM Get(int id) => ItemsCache.Lookup(id)
		.ValueOrThrow(() => new Exception($"Not found {nameof(ModelVM)} with id {id}"));

	public bool Contains(ModelVM modelVM)
	{
		Optional<ModelVM> lookup = ItemsCache.Lookup(modelVM.Model.Id);
		return lookup.HasValue && lookup.Value == modelVM;
	}

	public void Add(ModelVM item)
	{
		_modelsRepository.Add(item.Model);
		ItemsCache.AddOrUpdate(item);
	}

	public void Remove(ModelVM item)
	{
		_modelsRepository.Remove(item.Model);
	}
	
	private readonly Repository<Model> _modelsRepository;
}
