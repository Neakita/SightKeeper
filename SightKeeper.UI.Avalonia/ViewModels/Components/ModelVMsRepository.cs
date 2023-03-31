using System;
using System.Collections.ObjectModel;
using DynamicData;
using DynamicData.Kernel;
using SightKeeper.Domain.Model.Abstract;
using SightKeeper.Domain.Services;
using SightKeeper.UI.Avalonia.ViewModels.Elements;

namespace SightKeeper.UI.Avalonia.ViewModels.Components;

public sealed class ModelVMsRepository : DynamicRepository<ModelViewModel>
{
	public ReadOnlyObservableCollection<ModelViewModel> Items { get; }

	public ISourceCache<ModelViewModel, int> ItemsCache { get; } =
		new SourceCache<ModelViewModel, int>(modelVM => modelVM.Model.Id);
	

	public ModelVMsRepository(DynamicRepository<Model> modelsRepository)
	{
		_modelsRepository = modelsRepository;
		
		modelsRepository.ItemsCache
			.Connect()
			.Transform(ModelViewModel.Create)
			.PopulateInto(ItemsCache);

		ItemsCache.Connect().Bind(out ReadOnlyObservableCollection<ModelViewModel> items).Subscribe();
		
		Items = items;
	}

	public ModelViewModel Get(int id) => ItemsCache.Lookup(id)
		.ValueOrThrow(() => new Exception($"Not found {nameof(ModelViewModel)} with id {id}"));

	public bool Contains(ModelViewModel modelVM)
	{
		Optional<ModelViewModel> lookup = ItemsCache.Lookup(modelVM.Model.Id);
		return lookup.HasValue && lookup.Value == modelVM;
	}

	public void Add(ModelViewModel item)
	{
		_modelsRepository.Add(item.Model);
		ItemsCache.AddOrUpdate(item);
	}

	public void Remove(ModelViewModel item)
	{
		_modelsRepository.Remove(item.Model);
	}
	
	private readonly Repository<Model> _modelsRepository;
}
