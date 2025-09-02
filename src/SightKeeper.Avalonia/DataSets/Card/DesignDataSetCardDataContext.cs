using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;

namespace SightKeeper.Avalonia.DataSets.Card;

internal sealed class DesignDataSetCardDataContext : DataSetCardDataContext
{
	public static DesignDataSetCardDataContext Instance => new("KF2", "kfSample1.jpg");

	public string Name { get; }
	public ImageDataContext? Image { get; }
	public ICommand EditCommand => new RelayCommand(() => { });
	public ICommand ExportCommand => new RelayCommand(() => { });
	public ICommand DeleteCommand => new RelayCommand(() => { });

	public DesignDataSetCardDataContext(string name, string? sampleImageFileName = null)
	{
		Name = name;
		if (sampleImageFileName != null)
			Image = new DesignImageDataContext(sampleImageFileName);
	}
}