using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Avalonia.Media;
using CommunityToolkit.Mvvm.Input;

namespace SightKeeper.Avalonia.DataSets.Dialogs.Tags;

internal sealed class DesignEditablePoserTagDataContext : EditablePoserTagDataContext
{
	public string Name { get; set; }
	public Color Color { get; set; } = Colors.Transparent;

	public IReadOnlyCollection<EditableTagDataContext> KeyPointTags { get; }

	public ICommand CreateKeyPointTagCommand => new RelayCommand(() => { });

	public DesignEditablePoserTagDataContext(string name, params IEnumerable<string> keyPointTagNames)
	{
		Name = name;
		KeyPointTags = keyPointTagNames.Select(keyPointTagName => new DesignEditableTagDataContext(keyPointTagName)).ToList();
	}
}