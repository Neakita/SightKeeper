using System;
using System.Collections.Generic;
using SightKeeper.Application;
using SightKeeper.Application.ImageSets.Editing;
using SightKeeper.Avalonia.Extensions;
using SightKeeper.Domain.Images;
using Vibrance;
using Vibrance.Changes;

namespace SightKeeper.Avalonia.ImageSets;

public sealed class ImageSetViewModelsObservableRepository : ObservableRepository<ImageSetViewModel>, IDisposable
{
	public ReadOnlyObservableList<ImageSetViewModel> Items { get; }

	public ImageSetViewModelsObservableRepository(ObservableRepository<ImageSet> repository, ImageSetEditor editor)
	{
		Items = repository.Items
			.Transform(library => new ImageSetViewModel(library))
			.ToObservableList();
		Items.ToDictionary(viewModel => viewModel.Value, out var viewModelsLookup);
		_viewModelsLookup = viewModelsLookup;
		editor.Edited.Subscribe(OnImageSetEdited);
	}

	private void OnImageSetEdited(ImageSet set)
	{
		var viewModel = _viewModelsLookup[set];
		viewModel.NotifyPropertiesChanged();
	}

	public void Dispose()
	{
		Items.Dispose();
	}

	private readonly IReadOnlyDictionary<ImageSet, ImageSetViewModel> _viewModelsLookup;
}