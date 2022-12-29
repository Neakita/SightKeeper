using SightKeeper.DAL.Domain.Abstract;

namespace SightKeeper.Backend.Models;

public interface IModelEditor<T> where T : Model
{
	T EditableModel { get; }

	void SaveChanges();
	void DiscardChanges();
}