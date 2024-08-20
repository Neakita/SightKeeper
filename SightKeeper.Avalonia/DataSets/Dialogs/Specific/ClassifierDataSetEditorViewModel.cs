using System.Collections.Generic;
using System.Linq;
using Avalonia.Collections;
using CommunityToolkit.Mvvm.Input;

namespace SightKeeper.Avalonia.DataSets.Dialogs.Specific;

internal sealed partial class ClassifierDataSetEditorViewModel : SpecificDataSetEditorViewModel
{
	public override string Header => "Classifier";

	public IReadOnlyCollection<TagViewModel> Tags => _tags;

	private readonly AvaloniaList<TagViewModel> _tags = new();

	[RelayCommand(CanExecute = nameof(CanAddTag))]
	private void AddTag(string name)
	{
		_tags.Add(new TagViewModel(name));
	}

	private bool CanAddTag(string name)
	{
		return !string.IsNullOrWhiteSpace(name) && Tags.All(tag => tag.Name != name);
	}
}