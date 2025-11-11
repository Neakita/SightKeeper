using System;
using SightKeeper.Application.Misc;
using Vibrance;
using Vibrance.Changes;

namespace SightKeeper.Avalonia.Annotation.Tooling.ImageSet;

public sealed class ImageSetViewModelsObservableListRepository : ObservableListRepository<ImageSetViewModel>, IDisposable
{
	public ReadOnlyObservableList<ImageSetViewModel> Items { get; }

	public ImageSetViewModelsObservableListRepository(ObservableListRepository<Domain.Images.ImageSet> listRepository)
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