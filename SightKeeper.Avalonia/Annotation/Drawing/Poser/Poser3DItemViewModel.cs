using System.Collections.Generic;
using Avalonia.Collections;
using CommunityToolkit.Diagnostics;
using SightKeeper.Application.Annotation;
using SightKeeper.Domain.DataSets.Poser;
using SightKeeper.Domain.DataSets.Poser3D;

namespace SightKeeper.Avalonia.Annotation.Drawing.Poser;

public sealed class Poser3DItemViewModel : PoserItemViewModel
{
	public override Poser3DItem Value { get; }
	public override PoserTag Tag => Value.Tag;
	public override IReadOnlyList<KeyPoint3DViewModel> KeyPoints => _keyPoints;

	public Poser3DItemViewModel(Poser3DItem value, BoundingEditor boundingEditor) : base(boundingEditor)
	{
		Value = value;
	}

	internal void AddKeyPoint(KeyPoint3DViewModel keyPoint)
	{
		_keyPoints.Add(keyPoint);
	}

	internal void RemoveKeyPoint(KeyPoint3DViewModel keyPoint)
	{
		var isRemoved = _keyPoints.Remove(keyPoint);
		Guard.IsTrue(isRemoved);
	}

	private readonly AvaloniaList<KeyPoint3DViewModel> _keyPoints = new();
}