using System;
using System.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SightKeeper.Avalonia.DataSets.Dialogs.Tags;
using SightKeeper.Avalonia.Dialogs;

namespace SightKeeper.Avalonia.DataSets.Dialogs;

internal abstract partial class DataSetDialogViewModel : DialogViewModel<bool>, IDisposable
{
	public DataSetEditorViewModel DataSetEditor { get; }
	public abstract TagsEditorViewModel TagsEditor { get; }
	public virtual DataSetTypePickerViewModel? TypePicker => null;

	public DataSetDialogViewModel(DataSetEditorViewModel dataSetEditor)
	{
		DataSetEditor = dataSetEditor;
		DataSetEditor.ErrorsChanged += OnDataSetEditorErrorsChanged;
	}

	public virtual void Dispose()
	{
		DataSetEditor.Dispose();
		TagsEditor.Dispose();
		DataSetEditor.ErrorsChanged -= OnDataSetEditorErrorsChanged;
	}

	protected sealed override bool DefaultResult => false;

	private void OnDataSetEditorErrorsChanged(object? sender, DataErrorsChangedEventArgs e)
	{
		ApplyCommand.NotifyCanExecuteChanged();
	}

	[RelayCommand(CanExecute = nameof(CanApply))]
	private void Apply()
	{
		Return(true);
	}

	private bool CanApply()
	{
		return !DataSetEditor.HasErrors && TagsEditor.IsValid;
	}
}