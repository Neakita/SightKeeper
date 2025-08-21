namespace SightKeeper.Application.Training.COCO;

internal sealed class COCOAnnotation
{
	public int Id { get; set; }
	public int ImageId { get; set; }
	public int CategoryId { get; set; }
	public double Area { get; set; }
	public double[][] Segmentation { get; set; } = Array.Empty<double[]>();
	public double[] Bbox { get; set; } = Array.Empty<double>();
	public bool Iscrowd { get; set; }
}