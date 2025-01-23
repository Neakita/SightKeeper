using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Controls.Templates;
using Avalonia.Headless;
using Avalonia.Headless.XUnit;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Xaml.Interactivity;
using CommunityToolkit.Diagnostics;
using FluentAssertions;
using NSubstitute;
using SightKeeper.Avalonia.Annotation.Drawing;
using SightKeeper.Domain.DataSets.Assets;

namespace SightKeeper.Avalonia.Tests.Annotation.Drawing;

public sealed class BoundingDrawingBehaviorTests
{

	[AvaloniaFact]
	public void ShouldExecuteCommand()
	{
		var command = Substitute.For<ICommand>();
		command.CanExecute(Arg.Any<Bounding>()).Returns(true);
		var behavior = CreateBehavior(command);
		_canvas = CreateCanvas(behavior);
		_window = PrepareWindow(_canvas);
		Bounding bounding = new(0.1, 0.2, 0.3, 0.4);
		DrawBoundingAtCanvas(bounding);
		AssertCommandReceivedExecuteWithBounding(command, bounding);
	}

	[AvaloniaFact]
	public void ShouldNotExecuteCommandWhenCanExecuteReturnsFalse()
	{
		var command = Substitute.For<ICommand>();
		command.CanExecute(Arg.Any<Bounding>()).Returns(false);
		var behavior = CreateBehavior(command);
		_canvas = CreateCanvas(behavior);
		_window = PrepareWindow(_canvas);
		Bounding bounding = new(0.1, 0.2, 0.3, 0.4);
		DrawBoundingAtCanvas(bounding);
		command.DidNotReceive().Execute(Arg.Any<Bounding>());
	}

	[AvaloniaFact]
	public void ShouldDisplay()
	{
		var command = Substitute.For<ICommand>();
		command.CanExecute(Arg.Any<Bounding>()).Returns(true);
		var drawingItemTemplate = CreateDrawingItemTemplate();
		var behavior = CreateBehavior(command, drawingItemTemplate);
		_canvas = CreateCanvas(behavior);
		_window = PrepareWindow(_canvas);
		Bounding bounding = new(0.1, 0.2, 0.3, 0.4);
		BeginDrawBoundingAtCanvas(bounding);
		_canvas.Children.Should().Contain(control => IsDisplays(control, bounding));
	}

	[AvaloniaFact]
	public void ShouldNotDisplayWhenCommandCanExecuteReturnsFalse()
	{
		var command = Substitute.For<ICommand>();
		command.CanExecute(Arg.Any<Bounding>()).Returns(false);
		var drawingItemTemplate = CreateDrawingItemTemplate();
		var behavior = CreateBehavior(command, drawingItemTemplate);
		_canvas = CreateCanvas(behavior);
		_window = PrepareWindow(_canvas);
		Bounding bounding = new(0.1, 0.2, 0.3, 0.4);
		BeginDrawBoundingAtCanvas(bounding);
		_canvas.Children.Should().NotContain(control => IsDisplays(control, bounding));
	}

	private Canvas? _canvas;
	private Window? _window;

	private static Window PrepareWindow(Canvas canvas)
	{
		Window window = new()
		{
			Content = canvas
		};
		window.Show();
		return window;
	}

	private static BoundingDrawingBehavior CreateBehavior(ICommand command, IDataTemplate? drawingItemTemplate = null)
	{
		BoundingDrawingBehavior behavior = new()
		{
			Command = command,
			DrawingItemTemplate = drawingItemTemplate
		};
		return behavior;
	}

	private static Canvas CreateCanvas(BoundingDrawingBehavior behavior)
	{
		Canvas canvas = new()
		{
			Width = 100,
			Height = 100,
			Background = Brushes.Aqua
		};
		Interaction.SetBehaviors(canvas, [behavior]);
		return canvas;
	}

	private void DrawBoundingAtCanvas(Bounding bounding)
	{
		Guard.IsNotNull(_canvas);
		Point startPoint = new(_canvas.Width * bounding.Left, _canvas.Height * bounding.Top);
		Point endPoint = new(_canvas.Width * bounding.Right, _canvas.Height * bounding.Bottom);
		Guard.IsNotNull(_window);
		SimulateMouseAtCanvas(_window.MouseDown, startPoint);
		SimulateMouseAtCanvas(_window.MouseUp, endPoint);
	}

	private void SimulateMouseAtCanvas(Action<Point, MouseButton, RawInputModifiers> mouseDown, Point point)
	{
		Guard.IsNotNull(_canvas);
		Guard.IsNotNull(_window);
		var startPoint = _canvas.TranslatePoint(point, _window);
		Guard.IsNotNull(startPoint);
		mouseDown(startPoint.Value, MouseButton.Left, RawInputModifiers.None);
	}

	private void SimulateMouseAtCanvas(Action<Point, RawInputModifiers> mouseDown, Point point)
	{
		Guard.IsNotNull(_canvas);
		Guard.IsNotNull(_window);
		var startPoint = _canvas.TranslatePoint(point, _window);
		Guard.IsNotNull(startPoint);
		mouseDown(startPoint.Value, RawInputModifiers.None);
	}

	private static void AssertCommandReceivedExecuteWithBounding(ICommand command, Bounding bounding)
	{
		BoundingApproximateEqualityComparer comparer = new();
		command.Received().Execute(Arg.Is<Bounding>(drawnBounding => comparer.Equals(drawnBounding, bounding)));
	}

	private static IDataTemplate CreateDrawingItemTemplate()
	{
		return new FuncDataTemplate(_ => true, (_, _) => new Rectangle());
	}

	private void BeginDrawBoundingAtCanvas(Bounding bounding)
	{
		Guard.IsNotNull(_canvas);
		Point startPoint = new(_canvas.Width * bounding.Left, _canvas.Height * bounding.Top);
		Point endPoint = new(_canvas.Width * bounding.Right, _canvas.Height * bounding.Bottom);
		Guard.IsNotNull(_window);
		SimulateMouseAtCanvas(_window.MouseDown, startPoint);
		SimulateMouseAtCanvas(_window.MouseMove, endPoint);
	}

	private bool IsDisplays(Control control, Bounding bounding)
	{
		Guard.IsNotNull(_canvas);
		var canvasWidth = _canvas.Bounds.Width;
		var canvasHeight = _canvas.Bounds.Height;
		Bounding displayedBounding = new(
			control.Bounds.Left / canvasWidth,
			control.Bounds.Top / canvasHeight,
			control.Bounds.Right / canvasWidth,
			control.Bounds.Bottom / canvasHeight);
		BoundingApproximateEqualityComparer comparer = new();
		return comparer.Equals(bounding, displayedBounding);
	}
}