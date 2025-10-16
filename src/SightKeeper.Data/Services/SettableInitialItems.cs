namespace SightKeeper.Data.Services;

internal interface SettableInitialItems<T>
{
	void EnsureCapacity(int capacity);
	T WrapAndInsert(T item);
}