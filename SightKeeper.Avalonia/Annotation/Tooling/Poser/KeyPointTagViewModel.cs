using System.Windows.Input;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Avalonia.Annotation.Tooling.Poser;

public sealed class KeyPointTagViewModel : KeyPointTagDataContext
{
	public Tag Tag { get; }
	public string Name => Tag.Name;
	public ICommand DeleteKeyPointCommand { get; }

	public KeyPointTagViewModel(Tag tag, ICommand deleteKeyPointCommand)
	{
		Tag = tag;
		DeleteKeyPointCommand = deleteKeyPointCommand;
	}
}