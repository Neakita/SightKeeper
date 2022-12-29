using SightKeeper.DAL.Domain.Common;

namespace SightKeeper.DAL.Domain.Abstract;

public abstract class Screenshot
{
	private const string DirectoryPath = "Data/Images";
	private const string Extension = "png";


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

	public int Id { get; }

	public DateTime CreationDate { get; }

	public Resolution Resolution { get; }

	public string FilePath => $"{DirectoryPath}/{Id}.{Extension}";
}