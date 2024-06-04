using Material.Icons;

namespace SightKeeper.Avalonia.MessageBoxDialog;

internal sealed class MessageBoxButtonDefinition
{
	public string Text { get; }
	public MaterialIconKind IconKind { get; }
	public bool IsDefault { get; }

	public MessageBoxButtonDefinition(string text, MaterialIconKind iconKind, bool isDefault = false)
	{
		Text = text;
		IconKind = iconKind;
		IsDefault = isDefault;
	}
}