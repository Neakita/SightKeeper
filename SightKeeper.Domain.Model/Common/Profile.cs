using SightKeeper.Domain.Model.Detector;

namespace SightKeeper.Domain.Model.Common;

public class Profile
{
	public Profile(string name, DetectorModel detectorModel)
	{
		Name = name;
		// ReSharper disable once VirtualMemberCallInConstructor
		DetectorModel = detectorModel;
	}


	private Profile(int id, string name, string description)
	{
		Id = id;
		Name = name;
		Description = description;
		// ReSharper disable once VirtualMemberCallInConstructor
		DetectorModel = null!;
	}

	public int Id { get; private set; }
	public string Name { get; set; }
	public string Description { get; set; } = string.Empty;
	public virtual Game? Game { get; set; }
	public virtual DetectorModel DetectorModel { get; set; }
}