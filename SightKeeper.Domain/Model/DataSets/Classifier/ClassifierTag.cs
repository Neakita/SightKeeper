using CommunityToolkit.Diagnostics;

namespace SightKeeper.Domain.Model.DataSets.Classifier;

public sealed class ClassifierTag : Tag
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

	public override uint Color { get; set; }
	public ClassifierTagsLibrary Library { get; }
	public ClassifierDataSet DataSet => Library.DataSet;

	internal ClassifierTag(string name, uint color, ClassifierTagsLibrary library)
	{
		_name = name;
		Color = color;
		Library = library;
	}

	private string _name;
}