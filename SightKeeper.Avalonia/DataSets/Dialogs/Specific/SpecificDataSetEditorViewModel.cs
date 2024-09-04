using System;
using System.Collections.Generic;
using System.Reactive.Subjects;
using SightKeeper.Application.DataSets.Tags;
using SightKeeper.Domain.Model.DataSets;

namespace SightKeeper.Avalonia.DataSets.Dialogs.Specific;

internal abstract class SpecificDataSetEditorViewModel : ViewModel, IDisposable
{
	public abstract string Header { get; }
	public abstract DataSetType DataSetType { get; }
	public BehaviorObservable<bool> IsValid { get; }
	public abstract IReadOnlyCollection<TagData> Tags { get; }

	public virtual void Dispose()
	{
		_isValid.Dispose();
	}

	protected SpecificDataSetEditorViewModel(bool isInitiallyValid)
	{
		_isValid = new BehaviorSubject<bool>(isInitiallyValid);
		IsValid = _isValid;
	}

	protected readonly BehaviorSubject<bool> _isValid;
}