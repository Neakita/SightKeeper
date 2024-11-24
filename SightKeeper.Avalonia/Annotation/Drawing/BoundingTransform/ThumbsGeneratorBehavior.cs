using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Xaml.Interactivity;
using CommunityToolkit.Diagnostics;
using SightKeeper.Avalonia.Extensions;

namespace SightKeeper.Avalonia.Annotation.Drawing.BoundingTransform;

internal sealed class ThumbsGeneratorBehavior : Behavior<Panel>
{
	public static readonly StyledProperty<Thumb?> MoveThumbProperty =
		AvaloniaProperty.Register<ThumbsGeneratorBehavior, Thumb?>(nameof(MoveThumb));

	public static readonly StyledProperty<IDataTemplate?> SideThumbTemplateProperty =
		AvaloniaProperty.Register<ThumbsGeneratorBehavior, IDataTemplate?>(nameof(SideThumbTemplate));

	public static readonly StyledProperty<IDataTemplate?> CornerThumbTemplateProperty =
		AvaloniaProperty.Register<ThumbsGeneratorBehavior, IDataTemplate?>(nameof(CornerThumbTemplate));

	public static readonly StyledProperty<IDataTemplate?> SideMoveThumbTemplateProperty =
		AvaloniaProperty.Register<ThumbsGeneratorBehavior, IDataTemplate?>(nameof(SideMoveThumbTemplate));

	public static readonly AttachedProperty<ITransform?> RotationRenderTransformProperty =
		AvaloniaProperty.RegisterAttached<Thumb, ITransform?>("RotationRenderTransform", typeof(ThumbsGeneratorBehavior));

	public static void SetRotationRenderTransform(Thumb element, ITransform? value)
	{
		element.SetValue(RotationRenderTransformProperty, value);
	}

	public static ITransform? GetRotationRenderTransform(Thumb element)
	{
		return element.GetValue(RotationRenderTransformProperty);
	}

	[ResolveByName]
	public Thumb? MoveThumb
	{
		get => GetValue(MoveThumbProperty);
		set => SetValue(MoveThumbProperty, value);
	}

	public IDataTemplate? SideThumbTemplate
	{
		get => GetValue(SideThumbTemplateProperty);
		set => SetValue(SideThumbTemplateProperty, value);
	}

	public IDataTemplate? CornerThumbTemplate
	{
		get => GetValue(CornerThumbTemplateProperty);
		set => SetValue(CornerThumbTemplateProperty, value);
	}

	public IDataTemplate? SideMoveThumbTemplate
	{
		get => GetValue(SideMoveThumbTemplateProperty);
		set => SetValue(SideMoveThumbTemplateProperty, value);
	}

	protected override void OnAttachedToVisualTree()
	{
		GenerateThumbs();
	}

	private void GenerateThumbs()
	{
		for (int i = 0; i < 4; i++)
		{
			var side = (Side)i;
			var sideThumb = CreateSideThumb(side);
			RotateThumb(sideThumb, i);
			AssociatedObject.Children.Add(sideThumb);

			var nextSide = (Side)((i + 1) % 4);
			var cornerThumb = CreateCornerThumb(side, nextSide);
			RotateThumb(cornerThumb, i);
			AssociatedObject.Children.Add(cornerThumb);
		}
	}

	private Thumb CreateSideThumb(Side side)
	{
		var thumb = (Thumb)SideThumbTemplate.Build(null);
		var (horizontalAlignment, verticalAlignment) = SideToAlignment(side);
		thumb.HorizontalAlignment = horizontalAlignment ?? HorizontalAlignment.Center;
		thumb.VerticalAlignment = verticalAlignment ?? VerticalAlignment.Center;
		return thumb;
	}

	private Thumb CreateCornerThumb(Side side, Side nextSide)
	{
		var thumb = (Thumb)CornerThumbTemplate.Build(null);
		var (horizontalAlignment, verticalAlignment) = SidesToAlignment(side, nextSide);
		thumb.HorizontalAlignment = horizontalAlignment;
		thumb.VerticalAlignment = verticalAlignment;
		return thumb;
	}

	private static (HorizontalAlignment?, VerticalAlignment?) SideToAlignment(Side side) => side switch
	{
		Side.Left => (HorizontalAlignment.Left, null),
		Side.Top => (null, VerticalAlignment.Top),
		Side.Right => (HorizontalAlignment.Right, null),
		Side.Bottom => (null, VerticalAlignment.Bottom),
		_ => throw new ArgumentOutOfRangeException(nameof(side), side, null)
	};

	private static (HorizontalAlignment, VerticalAlignment) SidesToAlignment(Side side1, Side side2)
	{
		var (horizontalAlignment1, verticalAlignment1) = SideToAlignment(side1);
		var (horizontalAlignment2, verticalAlignment2) = SideToAlignment(side2);
		HorizontalAlignment horizontalAlignment;
		if (horizontalAlignment1 != null)
			horizontalAlignment = horizontalAlignment1.Value;
		else
		{
			Guard.IsNotNull(horizontalAlignment2);
			horizontalAlignment = horizontalAlignment2.Value;
		}
		VerticalAlignment verticalAlignment;
		if (verticalAlignment1 != null)
			verticalAlignment = verticalAlignment1.Value;
		else
		{
			Guard.IsNotNull(verticalAlignment2);
			verticalAlignment = verticalAlignment2.Value;
		}
		return (horizontalAlignment, verticalAlignment);
	}

	private static void RotateThumb(Thumb thumb, int degree)
	{
		if (degree == 0)
			return;
		RotateTransform transform = new(90 * degree);
		SetRotationRenderTransform(thumb, transform);
		Span<double> marginValues = stackalloc double[4];
		marginValues[0] = thumb.Margin.Left;
		marginValues[1] = thumb.Margin.Top;
		marginValues[2] = thumb.Margin.Right;
		marginValues[3] = thumb.Margin.Bottom;
		marginValues.Scroll(degree);
		thumb.Margin = new Thickness(marginValues[0], marginValues[1], marginValues[2], marginValues[3]);
	}
}