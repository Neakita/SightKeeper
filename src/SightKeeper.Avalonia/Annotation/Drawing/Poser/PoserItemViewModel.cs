using System;
using SightKeeper.Avalonia.Annotation.Drawing.Bounded;
using SightKeeper.Domain.DataSets.Poser;
using Vibrance;
using Vibrance.Changes;

namespace SightKeeper.Avalonia.Annotation.Drawing.Poser;

public sealed class PoserItemViewModel : BoundedItemViewModel, IDisposable
{
	public override PoserItem Value { get; }
	public override PoserTag Tag => Value.Tag;
	public ReadOnlyObservableList<KeyPointViewModel> KeyPoints { get; }

	public PoserItemViewModel(PoserItem value)
	{
		Value = value;
		var observableKeyPoints = (ReadOnlyObservableList<KeyPoint>)value.KeyPoints;
		KeyPoints = observableKeyPoints.Transform(CreateKeyPointViewModel).ToObservableList();
	}

	public void Dispose()
	{
		KeyPoints.Dispose();
	}

	private KeyPointViewModel CreateKeyPointViewModel(KeyPoint keyPoint)
	{
		return new KeyPointViewModel(this, keyPoint);
	}
}