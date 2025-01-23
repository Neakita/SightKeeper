using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using CommunityToolkit.Diagnostics;
using CommunityToolkit.Mvvm.Input;
using SightKeeper.Application.Annotation;
using SightKeeper.Domain.DataSets.Poser;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Avalonia.Annotation.Tooling;

public sealed partial class PoserTooling : ViewModel, IDisposable
{
	public TagSelection TagSelection => _tagSelection;
	public TagSelection KeyPointTagSelection => _keyPointTagSelection;

	public PoserItem? SelectedItem
	{
		get;
		set
		{
			field = value;
			_keyPointTagSelection.Tags = value?.Tag.KeyPointTags ?? ReadOnlyCollection<Tag>.Empty;
			DeleteKeyPointCommand.NotifyCanExecuteChanged();
		}
	}

	public PoserTooling(PoserAnnotator poserAnnotator, ObservablePoserAnnotator observablePoserAnnotator)
	{
		_poserAnnotator = poserAnnotator;
		_disposable = observablePoserAnnotator.KeyPointCreated
			.Merge(observablePoserAnnotator.KeyPointDeleted)
			.Where(data => data.item == SelectedItem)
			.Subscribe(_ => DeleteKeyPointCommand.NotifyCanExecuteChanged());
	}

	public void Dispose()
	{
		_disposable.Dispose();
	}

	private readonly TagSelectionViewModel _tagSelection = new();
	private readonly TagSelectionViewModel _keyPointTagSelection = new();
	private readonly PoserAnnotator _poserAnnotator;
	private readonly IDisposable _disposable;

	[RelayCommand(CanExecute = nameof(CanDeleteKeyPoint))]
	private void DeleteKeyPoint(Tag tag)
	{
		Guard.IsNotNull(SelectedItem);
		_poserAnnotator.DeleteKeyPoint(SelectedItem, tag);
	}

	private bool CanDeleteKeyPoint(Tag tag)
	{
		return SelectedItem != null && SelectedItem.KeyPoints.Any(keyPoint => keyPoint.Tag == tag);
	}
}