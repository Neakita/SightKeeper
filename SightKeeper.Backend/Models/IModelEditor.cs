using SightKeeper.Abstractions.Domain;

namespace SightKeeper.Backend.Models;

public interface IModelEditor<T> where T : class, IModel
{
	T EditableModel { get; }

	void SaveChanges();
	void DiscardChanges();
}