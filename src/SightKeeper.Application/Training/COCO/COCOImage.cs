namespace SightKeeper.Application.Training.COCO;

internal sealed class COCOImage
{
	public int Id { get; set; }
	public int Width { get; set; }
	public int Height { get; set; }
	public string FileName { get; set; } = string.Empty;
	public int License { get; set; }
	public string FlickrUrl { get; set; } = string.Empty;
	public string CocoUrl { get; set; } = string.Empty;
	public DateTime DateCaptured { get; set; }
}