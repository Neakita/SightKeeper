using System.ComponentModel;
using System.Runtime.CompilerServices;
using SightKeeper.Domain;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.DataSets.Weights;

namespace SightKeeper.Data.DataSets.Decorators;

internal sealed class NotifyingDataSet<TTag, TAsset>(DataSet<TTag, TAsset> inner) : DataSet<TTag, TAsset>, Decorator<DataSet<TTag, TAsset>>, INotifyPropertyChanged
{
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

	public TagsOwner<TTag> TagsLibrary => inner.TagsLibrary;
	public AssetsOwner<TAsset> AssetsLibrary => inner.AssetsLibrary;
	public WeightsLibrary WeightsLibrary => inner.WeightsLibrary;
	public DataSet<TTag, TAsset> Inner => inner;

	private void OnPropertyChanged([CallerMemberName] string propertyName = "")
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}