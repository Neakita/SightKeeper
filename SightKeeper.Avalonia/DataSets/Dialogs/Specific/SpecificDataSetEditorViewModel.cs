using System;
using System.Reactive.Subjects;

namespace SightKeeper.Avalonia.DataSets.Dialogs.Specific;

internal abstract class SpecificDataSetEditorViewModel : ViewModel, IDisposable
{
	public abstract string Header { get; }
	public BehaviorObservable<bool> IsValid { get; }

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