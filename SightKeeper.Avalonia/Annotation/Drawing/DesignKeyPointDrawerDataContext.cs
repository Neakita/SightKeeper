using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using SightKeeper.Avalonia.Annotation.Drawing.Poser;

namespace SightKeeper.Avalonia.Annotation.Drawing;

internal sealed class DesignKeyPointDrawerDataContext : KeyPointDrawerDataContext
{
	public ICommand CreateKeyPointCommand => new RelayCommand(() => { }, () => _isCommandEnabled);
	public KeyPointViewModel? ExistingKeyPoint => null;

	public DesignKeyPointDrawerDataContext(bool isCommandEnabled)
	{
		_isCommandEnabled = isCommandEnabled;
	}

	private readonly bool _isCommandEnabled;
}