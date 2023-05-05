using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations.Schema;
using ReactiveUI.Fody.Helpers;
using SightKeeper.Domain.Model.Abstract;
using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Domain.Model.Detector;

[Table("DetectorScreenshots")]
public class DetectorScreenshot : Screenshot
{
	public DetectorScreenshot(Image image) : base(image)
	{
		Items = new ObservableCollection<DetectorItem>();
	}
	
	private DetectorScreenshot(int id) : base(id)
	{
		Items = null!;
	}

	private void ItemsOnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
	{
		
	}

	[Reactive] public bool IsAsset { get; set; }

	public ObservableCollection<DetectorItem> Items { get; set; }
}