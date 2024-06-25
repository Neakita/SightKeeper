using CommunityToolkit.Diagnostics;

namespace SightKeeper.Domain.Model.DataSets.Detector;

public sealed class DetectorTag : Tag
{
	public override string Name
	{
		get => _name;
		set
		{
			if (_name == value)
				return;
			bool isNewNameOccupied = Library.Any(tag => tag.Name == value);
			Guard.IsFalse(isNewNameOccupied);
			_name = value;
		}
	}

	public DetectorTagsLibrary Library { get; }
	public DetectorDataSet DataSet => Library.DataSet;

	internal DetectorTag(string name, uint color, DetectorTagsLibrary library)
	{
		_name = name;
		Color = color;
		Library = library;
	}

	private string _name;
}