using System;
using SightKeeper.Application;
using SightKeeper.Domain.Images;
using Vibrance;
using Vibrance.Changes;

namespace SightKeeper.Avalonia.Annotation.Tooling;

public sealed class ImageSetViewModelsObservableListRepository : ObservableListRepository<ImageSetViewModel>, IDisposable
{
	public ReadOnlyObservableList<ImageSetViewModel> Items { get; }

	public ImageSetViewModelsObservableListRepository(ObservableListRepository<ImageSet> listRepository)
	{
		Items = listRepository.Items
			.Transform(library => new ImageSetViewModel(library))
			.ToObservableList();
	}

	public void Dispose()
	{
		Items.Dispose();
	}
}