namespace SightKeeper.Application.Training.COCO;

internal sealed class COCOCategory
{
	public int Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public string Supercategory { get; set; } = string.Empty;
}