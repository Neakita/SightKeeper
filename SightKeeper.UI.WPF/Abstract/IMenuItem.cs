using System.Windows.Controls;

namespace SightKeeper.UI.WPF.Abstract;

public interface IMenuItem
{
	string Label { get; }
	Control Icon { get; }
}