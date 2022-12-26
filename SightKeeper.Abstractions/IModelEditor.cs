using SightKeeper.Abstractions.Domain;

namespace SightKeeper.Abstractions;

public interface IModelEditor<T> where T : class, IModel
{
	T EditableModel { get; }

	void SaveChanges();
	void DiscardChanges();
}