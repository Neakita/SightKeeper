using MemoryPack;
using SightKeeper.Data.Model;
using SightKeeper.Domain;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data;

internal sealed class ImageSetFormatter : MemoryPackFormatter<ImageSet>
{
	public ImageSetFormatter(ChangeListener changeListener, Lock editingLock)
	{
		_wrapper = new ImageSetWrapper(changeListener, editingLock);
	}

	public override void Serialize<TBufferWriter>(ref MemoryPackWriter<TBufferWriter> writer, scoped ref ImageSet? value)
	{
		if (value == null)
		{
			writer.WriteNullObjectHeader();
			return;
		}
		var packable = AsPackable(value);
		writer.WritePackable(packable);
	}

	public override void Deserialize(ref MemoryPackReader reader, scoped ref ImageSet? value)
	{
		var packable = reader.ReadPackable<PackableImageSet>();
		if (packable == null)
		{
			value = null;
			return;
		}
		value = _wrapper.Wrap(packable);
	}

	private static PackableImageSet AsPackable(ImageSet set)
	{
		if (set is PackableImageSet packable)
			return packable;
		if (set is Decorator<ImageSet> facade)
			return AsPackable(facade.Inner);
		throw new ArgumentException($"Could not find packable implementation of {nameof(ImageSet)}", nameof(set));
	}

	private readonly ImageSetWrapper _wrapper;
}