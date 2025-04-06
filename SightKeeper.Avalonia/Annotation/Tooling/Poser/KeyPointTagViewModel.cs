using System.Windows.Input;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Avalonia.Annotation.Tooling.Poser;

internal sealed class KeyPointTagViewModel : KeyPointTagDataContext
{
	public string Name => _tag.Name;
	public ICommand DeleteKeyPointCommand { get; }

	public KeyPointTagViewModel(Tag tag, ICommand deleteKeyPointCommand)
	{
		_tag = tag;
		DeleteKeyPointCommand = deleteKeyPointCommand;
	}

	private readonly Tag _tag;
}