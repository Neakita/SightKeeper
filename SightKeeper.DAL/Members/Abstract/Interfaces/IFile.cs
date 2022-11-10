namespace SightKeeper.DAL.Members.Abstract.Interfaces;

public interface IFile
{
	string FilePath { get; }
	DateTime CreationDate { get; }
}
