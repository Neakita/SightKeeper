﻿namespace SightKeeper.Domain.Model.DataSets.Poser;

public sealed class PoserScreenshot : Screenshot
{
	public override PoserAsset? Asset => _asset;
	public override PoserScreenshotsLibrary Library { get; }

	internal PoserScreenshot(PoserScreenshotsLibrary library)
	{
		Library = library;
	}

	internal void SetAsset(PoserAsset? asset)
	{
		_asset = asset;
	}

	private PoserAsset? _asset;
}