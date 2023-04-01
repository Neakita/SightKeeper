namespace SightKeeper.UI.Avalonia.ViewModels.Elements;

public interface ItemVM<out TItem>
{
	TItem Item { get; }
}