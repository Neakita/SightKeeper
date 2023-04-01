using Microsoft.EntityFrameworkCore;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace SightKeeper.Domain.Model.Common;

[Owned]
public class Resolution : ReactiveObject
{
	public Resolution(ushort width = 320, ushort height = 320)
	{
		Width = width;
		Height = height;
	}

	[Reactive] public ushort Width { get; set; }

	[Reactive] public ushort Height { get; set; }

	public override string ToString() => $"{Width}x{Height}";
}