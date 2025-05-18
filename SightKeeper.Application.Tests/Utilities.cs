using NSubstitute;
using SightKeeper.Application.ImageSets;

namespace SightKeeper.Application.Tests;

internal static class Utilities
{
	public static ImageSetData CreateImageSetData(string name, string description = "")
	{
		var data = Substitute.For<ImageSetData>();
		data.Name.Returns(name);
		data.Description.Returns(description);
		return data;
	}
}