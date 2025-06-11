using MemoryPack;

namespace SightKeeper.Data;

public static class DataBootstrapper
{
	public static void Setup(ChangeListener changeListener, Lock editingLock)
	{
		MemoryPackFormatterProvider.Register(new ImageSetFormatter(changeListener, editingLock));
	}
}