using System.ComponentModel;
using System.Runtime.CompilerServices;
using SightKeeper.Data.DataSets.Tags;
using SightKeeper.Data.ImageSets.Images;
using SightKeeper.Domain.DataSets.Assets;

namespace SightKeeper.Data.DataSets.Assets.Items;

internal sealed class NotifyingItemsAsset<TItem>(StorableItemsAsset<TItem> inner) : StorableItemsAsset<TItem>, INotifyPropertyChanged
{
	public event PropertyChangedEventHandler? PropertyChanged;

	public StorableImage Image => inner.Image;

	public StorableItemsAsset<TItem> Innermost => inner.Innermost;

	public AssetUsage Usage
	{
		get => inner.Usage;
		set
		{
			inner.Usage = value;
			OnPropertyChanged();
		}
	}

	public TItem MakeItem(StorableTag tag)
	{
		return inner.MakeItem(tag);
	}

	public IReadOnlyList<TItem> Items => inner.Items;

	public void DeleteItemAt(int index)
	{
		inner.DeleteItemAt(index);
	}

	public void ClearItems()
	{
		inner.ClearItems();
	}

	private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}