using MemoryPack;
using SightKeeper.Data.Model.Images;
using SightKeeper.Domain.Images;
using PackableImageSet = SightKeeper.Data.Model.Images.PackableImageSet;

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
		var storable = (StorableImageSet)set;
		return storable.Packable;
	}

	private readonly ImageSetWrapper _wrapper;
}