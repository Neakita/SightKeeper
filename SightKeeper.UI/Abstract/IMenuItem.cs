namespace SightKeeper.UI.Abstract;

public interface IMenuItem
{
	string Label { get; }
	object Icon { get; }
	bool IsSelected { get; set; }
}