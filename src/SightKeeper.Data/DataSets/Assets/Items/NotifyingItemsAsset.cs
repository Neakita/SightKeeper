using System.ComponentModel;
using System.Runtime.CompilerServices;
using SightKeeper.Domain;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Assets.Items;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.DataSets.Assets.Items;

internal sealed class NotifyingItemsAsset<TItem>(ItemsAsset<TItem> inner) : ItemsAsset<TItem>, Decorator<ItemsAsset<TItem>>, INotifyPropertyChanged
{
	public event PropertyChangedEventHandler? PropertyChanged;

	public ManagedImage Image => inner.Image;

	public AssetUsage Usage
	{
		get => inner.Usage;
		set
		{
			inner.Usage = value;
			OnPropertyChanged();
		}
	}

	public TItem MakeItem(Tag tag)
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

	public ItemsAsset<TItem> Inner => inner;

	private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}