using System;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.Xaml.Interactivity;
using CommunityToolkit.Diagnostics;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.DataSets.Assets;

namespace SightKeeper.Avalonia.Annotation.Drawing.BoundingTransform;

internal sealed class DragableBoundingBehavior : Behavior<Control>
{
	public static readonly StyledProperty<Control?> ContainerProperty =
		AvaloniaProperty.Register<DragableBoundingBehavior, Control?>(nameof(Container));

	public static readonly StyledProperty<Bounding> ActualBoundingProperty =
		AvaloniaProperty.Register<DragableBoundingBehavior, Bounding>(nameof(ActualBounding),
			defaultBindingMode: BindingMode.TwoWay);

	public static readonly StyledProperty<Bounding> DisplayBoundingProperty =
		AvaloniaProperty.Register<DragableBoundingBehavior, Bounding>(nameof(DisplayBounding),
			defaultBindingMode: BindingMode.OneWayToSource);

	public static readonly StyledProperty<IDataTemplate?> ThumbTemplateProperty =
		AvaloniaProperty.Register<DragableBoundingBehavior, IDataTemplate?>(nameof(ThumbTemplate));

	public static readonly StyledProperty<Panel?> ThumbsPanelProperty =
		AvaloniaProperty.Register<DragableBoundingBehavior, Panel?>(nameof(ThumbsPanel));

	protected override void OnAttachedToVisualTree()
	{
		GenerateThumbs();
	}

	protected override void OnDetachedFromVisualTree()
	{
		foreach (var thumb in ThumbsPanel.Children.OfType<Thumb>())
		{
			thumb.DragStarted -= OnThumbDragStarted;
		}
	}

	[ResolveByName]
	public Control? Container
	{
		get => GetValue(ContainerProperty);
		set => SetValue(ContainerProperty, value);
	}

	public Bounding ActualBounding
	{
		get => GetValue(ActualBoundingProperty);
		set => SetValue(ActualBoundingProperty, value);
	}

	public Bounding DisplayBounding
	{
		get => GetValue(DisplayBoundingProperty);
		set => SetValue(DisplayBoundingProperty, value);
	}

	public IDataTemplate? ThumbTemplate
	{
		get => GetValue(ThumbTemplateProperty);
		set => SetValue(ThumbTemplateProperty, value);
	}

	[ResolveByName]
	public Panel? ThumbsPanel
	{
		get => GetValue(ThumbsPanelProperty);
		set => SetValue(ThumbsPanelProperty, value);
	}

	protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
	{
		base.OnPropertyChanged(change);
		if (change is AvaloniaPropertyChangedEventArgs<Thumb?> thumbChange)
		{
			var oldThumb = thumbChange.OldValue.Value;
			if (oldThumb != null)
				oldThumb.DragStarted -= OnThumbDragStarted;
			var newThumb = thumbChange.NewValue.Value;
			if (newThumb != null)
				newThumb.DragStarted += OnThumbDragStarted;
		}

		if (change.Property == ActualBoundingProperty)
			DisplayBounding = ActualBounding;
	}

	private void OnThumbDragStarted(object? sender, VectorEventArgs e)
	{
		Guard.IsNotNull(sender);
		var thumb = (Thumb)sender;
		thumb.DragDelta += OnThumbDragDelta;
		thumb.DragCompleted += OnThumbDragCompleted;
		Guard.IsNull(_transformer);
		_transformer = CreateTransformer(thumb.HorizontalAlignment, thumb.VerticalAlignment);
		var containerSize = Container.Bounds.Size;
		_transformer.MinimumSize = new Vector2<double>(1 / containerSize.Width * 20, 1 / containerSize.Height * 20);
	}

	private void OnThumbDragDelta(object? sender, VectorEventArgs e)
	{
		Guard.IsNotNull(_transformer);
		Guard.IsNotNull(Container);
		var containerSize = Container.Bounds.Size;
		DisplayBounding = _transformer.Transform(DisplayBounding,
			new Vector2<double>(e.Vector.X / containerSize.Width, e.Vector.Y / containerSize.Height));
	}

	private void OnThumbDragCompleted(object? sender, VectorEventArgs e)
	{
		Guard.IsNotNull(sender);
		var thumb = (Thumb)sender;
		thumb.DragDelta -= OnThumbDragDelta;
		thumb.DragCompleted -= OnThumbDragCompleted;
		ActualBounding = DisplayBounding;
		_transformer = null;
	}

	private static BoundingTransformer CreateTransformer(
		HorizontalAlignment horizontalAlignment,
		VerticalAlignment verticalAlignment) =>
		(horizontalAlignment, verticalAlignment) switch
		{
			(HorizontalAlignment.Left, VerticalAlignment.Top) => new AggregateBoundingTransformer(
				new BoundingSideTransformer(Side.Left), new BoundingSideTransformer(Side.Top)),
			(HorizontalAlignment.Right, VerticalAlignment.Top) => new AggregateBoundingTransformer(
				new BoundingSideTransformer(Side.Right), new BoundingSideTransformer(Side.Top)),
			(HorizontalAlignment.Left, VerticalAlignment.Bottom) => new AggregateBoundingTransformer(
				new BoundingSideTransformer(Side.Left), new BoundingSideTransformer(Side.Bottom)),
			(HorizontalAlignment.Right, VerticalAlignment.Bottom) => new AggregateBoundingTransformer(
				new BoundingSideTransformer(Side.Right), new BoundingSideTransformer(Side.Bottom)),
			(HorizontalAlignment.Left, VerticalAlignment.Center) => new BoundingSideTransformer(Side.Left),
			(HorizontalAlignment.Center, VerticalAlignment.Top) => new BoundingSideTransformer(Side.Top),
			(HorizontalAlignment.Right, VerticalAlignment.Center) => new BoundingSideTransformer(Side.Right),
			(HorizontalAlignment.Center, VerticalAlignment.Bottom) => new BoundingSideTransformer(Side.Bottom),
			(HorizontalAlignment.Stretch, VerticalAlignment.Top or VerticalAlignment.Center or VerticalAlignment.Bottom)
				=> new HorizontalMoveBoundingTransformer(),
			(HorizontalAlignment.Left or HorizontalAlignment.Center or HorizontalAlignment.Right, VerticalAlignment
				.Stretch) => new VerticalMoveBoundingTransformer(),
			(HorizontalAlignment.Center, VerticalAlignment.Center)
				or (HorizontalAlignment.Stretch, VerticalAlignment.Stretch) => new MoveBoundingTransformer(),
			_ => throw new ArgumentOutOfRangeException()
		};

	private BoundingTransformer? _transformer;

	private void GenerateThumbs()
	{
		ReadOnlySpan<HorizontalAlignment> horizontalAlignments =
		[
			HorizontalAlignment.Stretch, HorizontalAlignment.Left, HorizontalAlignment.Center, HorizontalAlignment.Right
		];
		ReadOnlySpan<VerticalAlignment> verticalAlignments =
		[
			VerticalAlignment.Stretch, VerticalAlignment.Top, VerticalAlignment.Center, VerticalAlignment.Bottom
		];
		foreach (var horizontalAlignment in horizontalAlignments)
		foreach (var verticalAlignment in verticalAlignments)
		{
			if (horizontalAlignment == HorizontalAlignment.Stretch && verticalAlignment == VerticalAlignment.Stretch)
				continue;
			if (horizontalAlignment == HorizontalAlignment.Stretch && verticalAlignment is VerticalAlignment.Center or VerticalAlignment.Bottom)
				continue;
			if (verticalAlignment == VerticalAlignment.Stretch && horizontalAlignment is HorizontalAlignment.Center or HorizontalAlignment.Right)
				continue;
			var thumb = (Thumb)ThumbTemplate.Build(null);
			thumb.DragStarted += OnThumbDragStarted;
			thumb.HorizontalAlignment = horizontalAlignment;
			thumb.VerticalAlignment = verticalAlignment;
			ThumbsPanel.Children.Add(thumb);
		}
	}
}