using SightKeeper.Domain.Model.Abstract;

namespace SightKeeper.Domain.Model.Detector;

public class DetectorAsset : Entity
{
	public Screenshot Screenshot { get; private set; }
	public ICollection<DetectorItem> Items { get; set; }
	
	public DetectorAsset(Screenshot screenshot)
	{
		Screenshot = screenshot;
		Items = new List<DetectorItem>();
	}
	
	private DetectorAsset(int id) : base(id)
	{
		Screenshot = null!;
		Items = null!;
	}
}