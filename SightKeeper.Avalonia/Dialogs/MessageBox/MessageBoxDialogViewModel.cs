using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Diagnostics;
using CommunityToolkit.Mvvm.Input;

namespace SightKeeper.Avalonia.Dialogs.MessageBox;

internal partial class MessageBoxDialogViewModel : DialogViewModel<MessageBoxButtonDefinition>
{
	public override string Header { get; }
	public string Message { get; }
	public IReadOnlyCollection<MessageBoxButtonDefinition> Buttons { get; }

	public MessageBoxDialogViewModel(string header, string message, params MessageBoxButtonDefinition[] buttons)
	{
		Guard.IsNotEmpty(buttons);
		Header = header;
		Message = message;
		Buttons = buttons;
		DefaultResult = buttons.SingleOrDefault(definition => definition.IsDefault) ?? buttons.First();
	}

	protected override MessageBoxButtonDefinition DefaultResult { get; }

	[RelayCommand]
	private void ReturnButtonDefinition(MessageBoxButtonDefinition buttonDefinition)
	{
		Return(buttonDefinition);
	}
}