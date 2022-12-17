using SightKeeper.DAL.Members.Abstract;

namespace SightKeeper.Backend.Models.Abstract;

public interface ModelEditor<T> where T : Model
{
	T EditableModel { get; }
	bool CanSaveChanges { get; }

	void SaveChanges();
	void DiscardChanges();
}