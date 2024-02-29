using CommunityToolkit.Mvvm.ComponentModel;
using FlakeId;
using SightKeeper.Domain.Model.DataSet.Screenshots.Assets;

namespace SightKeeper.Domain.Model.DataSet.Screenshots;

public sealed class Screenshot : ObservableObject
{
	public Id Id { get; private set; }
	public Image Image { get; private set; }
	public DateTime CreationDate { get; private set; }

	public Asset? Asset
	{
		get => _asset;
		internal set => SetProperty(ref _asset, value);
	}

	public ScreenshotsLibrary Library { get; private set; }

	internal Screenshot(byte[] content, ScreenshotsLibrary library)
	{
		Library = library;
		Image = new Image(content);
		CreationDate = DateTime.Now;
	}
	
	private Asset? _asset;

	private Screenshot()
	{
		Image = null!;
		Library = null!;
	}
}