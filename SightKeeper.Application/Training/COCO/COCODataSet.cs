namespace SightKeeper.Application.Training.COCO;

internal sealed class COCODataSet
{
	public COCODataSetInfo Info { get; set; } = new();
	public COCOImage[] Images { get; set; } = Array.Empty<COCOImage>();
	public COCOAnnotation[] Annotations { get; set; } = Array.Empty<COCOAnnotation>();
	public COCOCategory[] Categories { get; set; } = Array.Empty<COCOCategory>();
	public COCOLicense[] Licenses { get; set; } = Array.Empty<COCOLicense>();
}