using System;
using System.Collections.ObjectModel;
using DynamicData;
using SightKeeper.Application;
using SightKeeper.Domain.Screenshots;

namespace SightKeeper.Avalonia.Screenshots;

internal sealed class ScreenshotsLibraryViewModelsObservableRepository : ObservableRepository<ScreenshotsLibraryViewModel>, IDisposable
{
	public override ReadOnlyObservableCollection<ScreenshotsLibraryViewModel> Items { get; }
	public override IObservableList<ScreenshotsLibraryViewModel> Source { get; }

	public ScreenshotsLibraryViewModelsObservableRepository(ObservableRepository<ScreenshotsLibrary> repository)
	{
		Source = repository.Source.Connect()
			.Transform(library => new ScreenshotsLibraryViewModel(library))
			.Bind(out var items)
			.AsObservableList();
		Items = items;
	}

	public void Dispose()
	{
		Source.Dispose();
	}
}