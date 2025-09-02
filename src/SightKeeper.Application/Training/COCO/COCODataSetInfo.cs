namespace SightKeeper.Application.Training.COCO;

internal sealed class COCODataSetInfo
{
	public int Year { get; set; }
	public string Version { get; set; } = string.Empty;
	public string Description { get; set; } = string.Empty;
	public string Contributor { get; set; } = string.Empty;
	public string Url { get; set; } = string.Empty;
	public DateTime DateCreated { get; set; }
}