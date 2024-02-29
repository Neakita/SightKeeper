﻿using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Application;

public interface ScreenCapture
{
	byte[] Capture();

	Game? Game { get; set; }
	public ushort? Resolution { get; set; }
}