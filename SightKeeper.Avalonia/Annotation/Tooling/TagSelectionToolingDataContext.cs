using System.Collections.Generic;
using System.ComponentModel;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Avalonia.Annotation.Tooling;

public interface TagSelectionToolingDataContext : INotifyPropertyChanged
{
	bool IsEnabled { get; }
	IReadOnlyCollection<Named> Tags { get; }
	Named? SelectedTag { get; set; }
}