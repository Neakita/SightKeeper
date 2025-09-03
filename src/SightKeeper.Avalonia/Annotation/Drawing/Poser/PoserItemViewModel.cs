using System.Collections.Generic;
using Avalonia.Collections;
using CommunityToolkit.Diagnostics;
using SightKeeper.Avalonia.Annotation.Drawing.Bounded;
using SightKeeper.Domain.DataSets.Poser;

namespace SightKeeper.Avalonia.Annotation.Drawing.Poser;

public sealed class PoserItemViewModel : BoundedItemViewModel
{
	public override PoserItem Value { get; }
	public override PoserTag Tag => Value.Tag;
	public IReadOnlyList<KeyPointViewModel> KeyPoints => _keyPoints;

	public PoserItemViewModel(PoserItem value)
	{
		Value = value;
	}

	internal void AddKeyPoint(KeyPointViewModel keyPoint)
	{
		_keyPoints.Add(keyPoint);
	}

	internal void RemoveKeyPoint(KeyPointViewModel keyPoint)
	{
		var isRemoved = _keyPoints.Remove(keyPoint);
		Guard.IsTrue(isRemoved);
	}

	private readonly AvaloniaList<KeyPointViewModel> _keyPoints = new();
}