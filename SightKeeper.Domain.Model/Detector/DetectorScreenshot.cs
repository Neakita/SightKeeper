using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations.Schema;
using SightKeeper.Domain.Model.Abstract;
using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Domain.Model.Detector;

[Table("DetectorScreenshots")]
public class DetectorScreenshot : Screenshot
{
	public DetectorScreenshot(DetectorModel model, Image image) : base(image)
	{
		Model = model;
		Items = new ObservableCollection<DetectorItem>();
	}
	
	private DetectorScreenshot(int id) : base(id)
	{
		Model = null!;
		Items = null!;
	}

	private void ItemsOnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
	{
		
	}

	public bool IsAsset { get; set; }

	public ObservableCollection<DetectorItem> Items { get; private set; }
	public DetectorModel Model { get; private set; }
}