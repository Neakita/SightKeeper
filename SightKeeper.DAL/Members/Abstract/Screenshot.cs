using SightKeeper.Abstract.Interfaces;
using SightKeeper.DAL.Members.Common;

namespace SightKeeper.DAL.Members.Abstract;

public abstract class Screenshot : Entity, IScreenshot
{
	private const string DirectoryPath = "Data/Images/";
	private const string Extension = ".png";
	
	
	public string FilePath => DirectoryPath + id + Extension;
	
	public DateTime CreationDate { get; private set; }
	
	public IResolution Resolution { get; private set; }

	public Screenshot(IResolution resolution)
	{
		Resolution = resolution;
		CreationDate = DateTime.UtcNow;
	}

	
	protected Screenshot(Guid id) : base(id) =>
		Resolution = new Resolution();
}