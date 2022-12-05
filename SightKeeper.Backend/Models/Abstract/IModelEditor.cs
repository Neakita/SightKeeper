namespace SightKeeper.Backend.Models.Abstract;

public interface IModelEditor
{
	bool CanSaveChanges { get; }
	
	void SaveChanges();
	void DiscardChanges();
}