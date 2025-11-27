using System.ComponentModel;
using System.Runtime.CompilerServices;
using SightKeeper.Domain;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Application.Training;

public sealed class TrainingArtifact : INotifyPropertyChanged
{
	public event PropertyChangedEventHandler? PropertyChanged;

	public required string FilePath { get; init; }
	public string FileName => Path.GetFileName(FilePath);

	public required DataSet<ReadOnlyTag, ReadOnlyAsset> DataSet { get; init; }
	public required string Model { get; init; }
	public required string Format { get; init; }
	public required Vector2<ushort> Resolution { get; init; }

	public EpochResult? Epoch
	{
		get;
		set => SetField(ref field, value);
	}

	public DateTime Timestamp
	{
		get;
		set => SetField(ref field, value);
	}

	public Stream OpenReadStream()
	{
		return File.OpenRead(FilePath);
	}

	private void SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
	{
		if (EqualityComparer<T>.Default.Equals(field, value))
			return;
		field = value;
		OnPropertyChanged(propertyName);
	}

	private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}