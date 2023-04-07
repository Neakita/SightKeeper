using System.Collections.Specialized;
using Avalonia;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using SightKeeper.Application.Annotating;
using SightKeeper.Domain.Model.Common;
using SightKeeper.Domain.Model.Detector;

namespace SightKeeper.Infrastructure.Services;

public sealed class DetectorAnnotatorImplementation : ReactiveObject, DetectorAnnotator
{
	private DetectorScreenshot? _selectedScreenshot;
	[Reactive] public DetectorModel? Model { get; set; }

	public DetectorScreenshot? SelectedScreenshot
	{
		get => _selectedScreenshot;
		set
		{
			this.RaiseAndSetIfChanged(ref _selectedScreenshot, value);
			value.Items.Add(new DetectorItem(new ItemClass(""), new BoundingBox(Random.Shared.NextDouble() / 2, 0.25, 0.1, 0.2)));
		}
	}

	[Reactive] public ItemClass? SelectedItemClass { get; set; }

	public void AddItem(Point position, Size size)
	{
		if (SelectedItemClass == null) throw new NullReferenceException("No selected item class");
		if (SelectedScreenshot == null) throw new NullReferenceException("No selected screenshot");
		DetectorItem item = new(SelectedItemClass, new BoundingBox(position, size, new Size()));
		SelectedScreenshot.Items.Add(item);
		MarkAsAsset(SelectedScreenshot);
	}

	public void AddItem(Point position, Size size, Size canvasSize)
	{
		throw new NotImplementedException();
	}

	public void RemoveItem(DetectorItem item)
	{
		if (SelectedScreenshot == null) throw new NullReferenceException("No selected screenshot");
		SelectedScreenshot.Items.Remove(item);
	}

	public void Move(DetectorItem item, Point position, Size size)
	{
		throw new NotImplementedException();
	}

	public void Move(DetectorItem item, Point position, Size size, Size canvasSize)
	{
		item.BoundingBox = new BoundingBox(position, size, canvasSize);
	}

	public void MarkAsAsset(DetectorScreenshot screenshot)
	{
		throw new NotImplementedException();
	}

	public void RemoveScreenshot(DetectorScreenshot screenshot)
	{
		throw new NotImplementedException();
	}

	public void RemoveScreenshots(IReadOnlyCollection<DetectorScreenshot> screenshots)
	{
		
	}
}
