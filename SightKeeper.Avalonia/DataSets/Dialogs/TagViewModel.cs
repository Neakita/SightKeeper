using Avalonia.Media;

namespace SightKeeper.Avalonia.DataSets.Dialogs;

internal class TagViewModel : ViewModel
{
	public string Name
	{
		get => _name;
		set => SetProperty(ref _name, value);
	}

	public Color Color
	{
		get => _color;
		set => SetProperty(ref _color, value);
	}

	public TagViewModel(string name)
	{
		_name = name;
	}

	private string _name;
	private Color _color;
}