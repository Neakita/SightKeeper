using Microsoft.EntityFrameworkCore;
using ReactiveUI.Fody.Helpers;
using ReactiveUI.Validation.Extensions;
using ReactiveUI.Validation.Helpers;

namespace SightKeeper.Domain.Model.Common;

[Owned]
public class Resolution : ReactiveValidationObject
{
	public Resolution(ushort width = 320, ushort height = 320)
	{
		Width = width;
		Height = height;

		this.ValidationRule(
			resolution => resolution.Width,
			w => w > 0,
			"Width must be greater than 0");
		
		this.ValidationRule(
			resolution => resolution.Height,
			h => h > 0,
			"Height must be greater than 0");

		this.ValidationRule(
			resolution => resolution.Width,
			w => w % 32 == 0,
			"Width must be multiplier of 32");
		
		this.ValidationRule(
			resolution => resolution.Height,
			h => h % 32 == 0,
			"Height must be multiplier of 32");
	}

	[Reactive] public ushort Width { get; set; }

	[Reactive] public ushort Height { get; set; }

	public override string ToString() => $"{Width}x{Height}";
}