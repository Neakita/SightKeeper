using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using CommunityToolkit.Diagnostics;
using CommunityToolkit.Mvvm.Input;

namespace SightKeeper.Avalonia.Dialogs.MessageBox;

internal class MessageBoxDialogViewModel : DialogViewModel<MessageBoxButtonDefinition>
{
	public override string Header { get; }
	public string Message { get; }
	public IReadOnlyCollection<MessageBoxButtonDefinition> Buttons { get; }
	public ICommand ReturnCommand => new RelayCommand<MessageBoxButtonDefinition>(definition =>
	{
		Guard.IsNotNull(definition);
		Return(definition);
	});

	public MessageBoxDialogViewModel(string header, string message, params MessageBoxButtonDefinition[] buttons)
	{
		Guard.IsNotEmpty(buttons);
		Header = header;
		Message = message;
		Buttons = buttons;
		DefaultResult = buttons.SingleOrDefault(definition => definition.IsDefault) ?? buttons.First();
	}

	protected override MessageBoxButtonDefinition DefaultResult { get; }
}