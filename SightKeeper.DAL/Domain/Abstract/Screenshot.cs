using SightKeeper.DAL.Domain.Common;

namespace SightKeeper.DAL.Domain.Abstract;

public abstract class Screenshot
{
	public Screenshot() : this(new Resolution())
	{
	}

	public Screenshot(Resolution resolution)
	{
		Resolution = resolution;
		CreationDate = DateTime.UtcNow;
	}


	protected Screenshot(int id, DateTime creationDate)
	{
		Id = id;
		CreationDate = creationDate;
		Resolution = new Resolution();
	}

	public int Id { get; private set; }

	public DateTime CreationDate { get; private set; }

	public Resolution Resolution { get; }
}