using CommunityToolkit.Diagnostics;
using SightKeeper.Domain.Model.Abstract;

namespace SightKeeper.Domain.Model.Detector;

public sealed class DetectorAsset : Asset
{
	public IReadOnlyCollection<DetectorItem> Items => _items;

	internal override Abstract.Model Model => _model;
	
	internal DetectorAsset(DetectorModel model, Screenshot screenshot) : base(screenshot)
	{
		_model = model;
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
		_items = null!;
	}
}