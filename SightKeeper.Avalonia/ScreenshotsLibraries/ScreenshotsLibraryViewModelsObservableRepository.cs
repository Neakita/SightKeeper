using System;
using System.Collections.ObjectModel;
using DynamicData;
using SightKeeper.Application;
using SightKeeper.Application.ImageSets.Editing;
using SightKeeper.Domain.Images;

namespace SightKeeper.Avalonia.ScreenshotsLibraries;

public sealed class ScreenshotsLibraryViewModelsObservableRepository : ObservableRepository<ScreenshotsLibraryViewModel>, IDisposable
{
	public override ReadOnlyObservableCollection<ScreenshotsLibraryViewModel> Items { get; }
	public override IObservableList<ScreenshotsLibraryViewModel> Source { get; }

	public ScreenshotsLibraryViewModelsObservableRepository(ObservableRepository<ImageSet> repository, ImageSetEditor editor)
	{
		Source = repository.Source.Connect()
			.Transform(library => new ScreenshotsLibraryViewModel(library))
			.Bind(out var items)
			.AsObservableList();
		Items = items;
		_cache = Source.Connect()
			.AddKey(viewModel => viewModel.Value)
			.AsObservableCache();
		editor.Edited.Subscribe(OnScreenshotsLibraryEdited);
	}

	private void OnScreenshotsLibraryEdited(ImageSet library)
	{
		var viewModel = _cache.Lookup(library).Value;
		viewModel.NotifyPropertiesChanged();
	}

	public void Dispose()
	{
		Source.Dispose();
	}

	private readonly IObservableCache<ScreenshotsLibraryViewModel, ImageSet> _cache;
}