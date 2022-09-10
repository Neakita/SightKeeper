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
	public ushort Width { get; set; }
	public ushort Height { get; set; }

	[NotMapped] public bool IsExists => System.IO.File.Exists(FilePath);

	[NotMapped] public Bitmap? File
	{
		get
		{
			if (!IsExists) return null;
			using MemoryStream memoryStream = new(System.IO.File.ReadAllBytes(FilePath));
			return new Bitmap(memoryStream);
		}
		set
		{
			if (value == null) EnsureDeleted();
			else
			{
				if (value.Width != Width) throw new Exception("The width of the image does not match the required");
				if (value.Height != Height) throw new Exception("The height of the image does not match the required");
				if (!Directory.Exists(DirectoryPath)) Directory.CreateDirectory(DirectoryPath);
				value.Save(FilePath);
			}
		}
	}

	public Screenshot()
	{
		Date = DateTime.Now;
	}
	
	public Screenshot(DateTime date) => Date = date;

	public void EnsureDeleted()
	{
		if (IsExists) System.IO.File.Delete(FilePath);
	}
}