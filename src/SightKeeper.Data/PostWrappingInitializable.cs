namespace SightKeeper.Data;

internal interface PostWrappingInitializable<in T>
{
	void Initialize(T wrapped);
}