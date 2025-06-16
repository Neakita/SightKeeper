using System.Collections.Generic;
using Avalonia.Collections;
using CommunityToolkit.Diagnostics;
using SightKeeper.Application.Annotation;
using SightKeeper.Domain.DataSets.Poser;
using SightKeeper.Domain.DataSets.Poser2D;

namespace SightKeeper.Avalonia.Annotation.Drawing.Poser;

public sealed class Poser2DItemViewModel : PoserItemViewModel
{
	public override DomainPoser2DItem Value { get; }
	public override DomainPoserTag Tag => Value.Tag;
	public override IReadOnlyList<KeyPoint2DViewModel> KeyPoints => _keyPoints;

	public Poser2DItemViewModel(DomainPoser2DItem value, BoundingEditor boundingEditor) : base(boundingEditor)
	{
		Value = value;
	}

	internal void AddKeyPoint(KeyPoint2DViewModel keyPoint)
	{
		_keyPoints.Add(keyPoint);
	}

	internal override void RemoveKeyPoint(KeyPointViewModel keyPoint)
	{
		RemoveKeyPoint((KeyPoint2DViewModel)keyPoint);
	}

	internal void RemoveKeyPoint(KeyPoint2DViewModel keyPoint)
	{
		var isRemoved = _keyPoints.Remove(keyPoint);
		Guard.IsTrue(isRemoved);
	}

	private readonly AvaloniaList<KeyPoint2DViewModel> _keyPoints = new();
}