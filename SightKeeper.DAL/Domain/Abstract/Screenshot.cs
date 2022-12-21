using System.ComponentModel.DataAnnotations;
using SightKeeper.DAL.Members.Common;

namespace SightKeeper.DAL.Members.Abstract;

public abstract class Screenshot
{
	private const string DirectoryPath = "Data/Images";
	private const string Extension = "png";
	
	[Key] public Guid Id { get; private set; }

	public DateTime CreationDate { get; private set; }
	
	public virtual Resolution Resolution { get; private set; }
	
	public string FilePath => $"{DirectoryPath}/{Id}.{Extension}";
	
	
	public Screenshot() : this(new Resolution()) { }

	public Screenshot(Resolution resolution)
	{
		Resolution = resolution;
		CreationDate = DateTime.UtcNow;
	}


	protected Screenshot(Guid id, DateTime creationDate)
	{
		Id = id;
		CreationDate = creationDate;
		Resolution = new Resolution();
	}
}