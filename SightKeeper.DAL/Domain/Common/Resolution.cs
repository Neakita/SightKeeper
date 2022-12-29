using Microsoft.EntityFrameworkCore;

namespace SightKeeper.DAL.Domain.Common;

/// <summary>
///     A class representing resolution of screenshots and models
/// </summary>
[Owned]
public class Resolution
{
	private readonly ushort _height;


	private readonly ushort _width;

	public Resolution(ushort width = 320, ushort height = 320)
	{
		Width = width;
		Height = height;
	}

	public ushort Width
	{
		get => _width;
		init
		{
			if (!IsValid(value))
				throw new ArgumentException($"Width must be a multiple of 32, but actually it is {value}",
					nameof(Width));
			_width = value;
		}
	}

	public ushort Height
	{
		get => _height;
		init
		{
			if (!IsValid(value))
				throw new ArgumentException($"Height must be a multiple of 32, but actually it is {value}",
					nameof(Height));
			_height = value;
		}
	}


	private static bool IsValid(ushort value) => value % 32 == 0;
}