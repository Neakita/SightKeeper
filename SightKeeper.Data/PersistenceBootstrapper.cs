using MemoryPack;
using SightKeeper.Data.Formatters;

namespace SightKeeper.Data;

public static class PersistenceBootstrapper
{
	public static void Setup(ChangeListener changeListener, Lock editingLock)
	{
		MemoryPackFormatterProvider.Register(new ImageFormatter());
		MemoryPackFormatterProvider.Register(new ImageSetFormatter(changeListener, editingLock));
	}
}