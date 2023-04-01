using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using SightKeeper.Domain.Model.Abstract;
using SightKeeper.Domain.Model.Detector;

namespace SightKeeper.Domain.Model.Common;

public class Profile : ReactiveObject, Entity
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
	[Reactive] public string Name { get; set; }
	[Reactive] public string Description { get; set; } = string.Empty;
	[Reactive] public virtual Game? Game { get; set; }
	[Reactive] public virtual DetectorModel DetectorModel { get; set; }
}