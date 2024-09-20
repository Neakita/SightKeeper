﻿using SightKeeper.Domain.Model;
using SixLabors.ImageSharp;

namespace SightKeeper.Application;

public interface ScreenCapture
{
	Image Capture(Vector2<ushort> resolution, Game? game);
}