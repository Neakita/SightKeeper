using System.IO.Compression;
using FlakeId;
using Serilog;

namespace SightKeeper.Data.Services;

internal sealed class CompressedFileSystemDataAccess(ILogger logger) : FileSystemDataAccess(logger)
{
	public CompressionLevel CompressionLevel { get; set; } = CompressionLevel.SmallestSize;

	public override Stream OpenRead(Id id)
	{
		var stream = base.OpenRead(id);
		return new ZLibStream(stream, CompressionMode.Decompress);
	}

	public override Stream OpenWrite(Id id)
	{
		var stream = base.OpenWrite(id);
		return new ZLibStream(stream, CompressionLevel);
	}
}