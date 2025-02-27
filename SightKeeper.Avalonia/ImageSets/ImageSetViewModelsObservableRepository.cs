using System;
using System.Collections.ObjectModel;
using DynamicData;
using SightKeeper.Application;
using SightKeeper.Application.ImageSets.Editing;
using SightKeeper.Domain.Images;

namespace SightKeeper.Avalonia.ImageSets;

public sealed class ImageSetViewModelsObservableRepository : ObservableRepository<ImageSetViewModel>, IDisposable
{
	public override ReadOnlyObservableCollection<ImageSetViewModel> Items { get; }
	public override IObservableList<ImageSetViewModel> Source { get; }

	public ImageSetViewModelsObservableRepository(ObservableRepository<ImageSet> repository, ImageSetEditor editor)
	{
		Source = repository.Source.Connect()
			.Transform(library => new ImageSetViewModel(library))
			.Bind(out var items)
			.AsObservableList();
		Items = items;
		_cache = Source.Connect()
			.AddKey(viewModel => viewModel.Value)
			.AsObservableCache();
		editor.Edited.Subscribe(OnImageSetEdited);
	}

	private void OnImageSetEdited(ImageSet set)
	{
		var viewModel = _cache.Lookup(set).Value;
		viewModel.NotifyPropertiesChanged();
	}

	public void Dispose()
	{
		Source.Dispose();
	}

	private readonly IObservableCache<ImageSetViewModel, ImageSet> _cache;
}