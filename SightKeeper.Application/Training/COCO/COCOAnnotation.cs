namespace SightKeeper.Application.Training.COCO;

internal sealed class COCOAnnotation
{
	public int Id { get; set; }
	public int ImageId { get; set; }
	public int CategoryId { get; set; }
	// segmentation and area are skipped
	public double[] Bbox { get; set; } = Array.Empty<double>();
	public bool Iscrowd { get; set; }
}