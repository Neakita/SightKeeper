using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;

namespace SightKeeper.Backend.Data.Members.Abstract;

public abstract class Screenshot
{
	private const string DirectoryPath = "Data/Images/";
	private const string Extension = ".png";
	private string FilePath => DirectoryPath + Id + Extension;
	
	public Guid Id { get; set; }
	public DateTime Date { get; set; }

	[NotMapped] public bool IsExists => File.Exists(FilePath);

	[NotMapped] public Bitmap? Bitmap
	{
		get
		{
			if (!IsExists) return null;
			using var memoryStream = new MemoryStream(File.ReadAllBytes(FilePath));
			return new Bitmap(memoryStream);
		}
		set
		{
			if (value == null) EnsureDeleted();
			else value.Save(FilePath);
		}
	}

	public Screenshot()
	{
		Date = DateTime.Now;
	}
	
	public Screenshot(DateTime date) => Date = date;

	public void EnsureDeleted()
	{
		if (IsExists) File.Delete(FilePath);
	}
}