namespace SightKeeper.Abstract.Interfaces;

public interface IFile
{
	string FilePath { get; }
	DateTime CreationDate { get; }
}
