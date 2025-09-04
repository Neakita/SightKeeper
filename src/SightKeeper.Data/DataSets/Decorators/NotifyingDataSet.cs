using System.ComponentModel;
using System.Runtime.CompilerServices;
using SightKeeper.Domain;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.DataSets.Weights;

namespace SightKeeper.Data.DataSets.Decorators;

internal sealed class NotifyingDataSet<TAsset>(DataSet<TAsset> inner) : DataSet<TAsset>, Decorator<DataSet<TAsset>>, INotifyPropertyChanged
{
	public DataSet<TAsset> Inner => inner;
	public event PropertyChangedEventHandler? PropertyChanged;

	public string Name
	{
		get => inner.Name;
		set
		{
			if (Name == value)
				return;
			inner.Name = value;
			OnPropertyChanged();
		}
	}

	public string Description
	{
		get => inner.Description;
		set
		{
			if (Description == value)
				return;
			inner.Description = value;
			OnPropertyChanged();
		}
	}

	public TagsOwner<Tag> TagsLibrary => inner.TagsLibrary;
	public AssetsOwner<TAsset> AssetsLibrary => inner.AssetsLibrary;
	public WeightsLibrary WeightsLibrary => inner.WeightsLibrary;

	private void OnPropertyChanged([CallerMemberName] string propertyName = "")
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}