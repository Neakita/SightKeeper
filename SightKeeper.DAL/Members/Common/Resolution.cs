using SightKeeper.Abstract.Interfaces;

namespace SightKeeper.DAL.Members.Common;

public sealed class Resolution : IResolution
{
	public ushort Width
	{
		get => _width;
		set
		{
			if (!IsValid(value)) throw new ArgumentException(InvalidWidthMessage, nameof(value));
			_width = value;
		}
	}

	public ushort Height
	{
		get => _height;
		set
		{
			if (!IsValid(value)) throw new ArgumentException(InvalidHeightMessage, nameof(value));
			_height = value;
		}
	}
	
	public Resolution(ushort width = 320, ushort height = 320)
	{
		if (!IsValid(width)) throw new ArgumentException(InvalidWidthMessage, nameof(width));
		if (!IsValid(height)) throw new ArgumentException(InvalidHeightMessage, nameof(height));
		
		Width = width;
		Height = height;
	}


	private const string InvalidWidthMessage = "The width must be a multiple of 32";
	private const string InvalidHeightMessage = "The height must be a multiple of 32";

	private ushort _width;
	private ushort _height;
	

	private static bool IsValid(ushort value) => value % 32 == 0;
}