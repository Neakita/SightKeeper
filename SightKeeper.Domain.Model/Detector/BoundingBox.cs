using Microsoft.EntityFrameworkCore;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace SightKeeper.Domain.Model.Detector;

[Owned]
public class BoundingBox : ReactiveObject
{
	public BoundingBox(float x, float y, float width, float height)
	{
		X = x;
		Y = y;
		Width = width;
		Height = height;
	}

	[Reactive] public float X { get; set; }
	[Reactive] public float Y { get; set; }
	[Reactive] public float Width { get; set; }
	[Reactive] public float Height { get; set; }
}