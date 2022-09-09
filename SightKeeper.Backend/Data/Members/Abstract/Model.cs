using System.ComponentModel.DataAnnotations.Schema;
using SightKeeper.Backend.Data.Members.Common;
using SightKeeper.Backend.Data.Members.Detector;

namespace SightKeeper.Backend.Data.Members.Abstract;

public abstract class Model
{
	public Guid Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public Resolution Resolution { get; set; } = new();
	public List<ItemClass> Classes { get; set; } = new();
	public Game? Game { get; set; }

	[NotMapped] public abstract IEnumerable<Screenshot> Screenshots { get; }
}