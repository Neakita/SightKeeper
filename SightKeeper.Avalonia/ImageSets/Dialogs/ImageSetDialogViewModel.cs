using System;
using System.Collections;
using System.ComponentModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SightKeeper.Avalonia.Dialogs;

namespace SightKeeper.Avalonia.ImageSets.Dialogs;

internal abstract partial class ImageSetDialogViewModel : DialogViewModel<bool>, ImageSetDialogDataContext, INotifyDataErrorInfo, IDisposable
{
	public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged
	{
		add => ErrorInfoSource.ErrorsChanged += value;
		remove => ErrorInfoSource.ErrorsChanged -= value;
	}

	[ObservableProperty] public partial string Name { get; set; } = string.Empty;
	[ObservableProperty] public partial string Description { get; set; } = string.Empty;

	public bool HasErrors => ErrorInfoSource.HasErrors;

	ICommand ImageSetDialogDataContext.CancelCommand => CloseCommand;

	public IEnumerable GetErrors(string? propertyName)
	{
		return ErrorInfoSource.GetErrors(propertyName);
	}

	public virtual void Dispose()
	{
		ErrorInfoSource.ErrorsChanged -= OnErrorsChanged;
	}

	protected override bool DefaultResult => false;
	protected abstract INotifyDataErrorInfo ErrorInfoSource { get; }

	protected void InitializeErrorHandling()
	{
		ErrorInfoSource.ErrorsChanged += OnErrorsChanged;
	}

	private void OnErrorsChanged(object? sender, DataErrorsChangedEventArgs e)
	{
		ApplyCommand.NotifyCanExecuteChanged();
	}

	[RelayCommand(CanExecute = nameof(CanApply))]
	private void Apply()
	{
		Return(true);
	}

	protected bool CanApply()
	{
		return !HasErrors;
	}
}