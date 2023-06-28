using CommunityToolkit.Diagnostics;
using SightKeeper.Domain.Model.Abstract;

namespace SightKeeper.Domain.Model.Detector;

public sealed class DetectorAsset
{
	public Screenshot Screenshot { get; private set; }
	public IReadOnlyCollection<DetectorItem> Items => _items;
	
	public DetectorAsset(DetectorModel model, Screenshot screenshot)
	{
		_model = model;
		Screenshot = screenshot;
		_items = new List<DetectorItem>();
	}

	public void AddItem(DetectorItem item)
	{
		if (!_model.ItemClasses.Contains(item.ItemClass))
			ThrowHelper.ThrowInvalidOperationException($"Model \"{_model}\" does not contain item class \"{item.ItemClass}\"");
		if (Items.Contains(item))
			ThrowHelper.ThrowInvalidOperationException("Item already added");
		_items.Add(item);
	}

	private readonly DetectorModel _model;
	private readonly List<DetectorItem> _items;

	private DetectorAsset()
	{
		_model = null!;
		Screenshot = null!;
		_items = null!;
	}
}