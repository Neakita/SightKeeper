using System.Collections.ObjectModel;

namespace SightKeeper.Application.Training.COCO;

internal sealed class COCODataSet
{
	public COCODataSetInfo Info { get; set; } = new();
	public IReadOnlyList<COCOImage> Images { get; set; } = ReadOnlyCollection<COCOImage>.Empty;
	public IReadOnlyList<COCOAnnotation> Annotations { get; set; } = ReadOnlyCollection<COCOAnnotation>.Empty;
	public IReadOnlyList<COCOCategory> Categories { get; set; } = ReadOnlyCollection<COCOCategory>.Empty;
	public IReadOnlyList<COCOLicense> Licenses { get; set; } = ReadOnlyCollection<COCOLicense>.Empty;
}