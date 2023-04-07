using System;
using Avalonia;
using Avalonia.Input;
using Avalonia.ReactiveUI;
using SightKeeper.UI.Avalonia.Extensions;
using SightKeeper.UI.Avalonia.ViewModels.Tabs;

namespace SightKeeper.UI.Avalonia.Views.Tabs;

public partial class AnnotatingTab : ReactiveUserControl<AnnotatingTabVM>
{
	public AnnotatingTab(AnnotatingTabVM viewModel) : this()
	{
		ViewModel = viewModel;
	}
	
	public AnnotatingTab()
	{
		InitializeComponent();
		KeyDown += OnKeyDown;
	}

	protected override void OnInitialized()
	{
		base.OnInitialized();
		Image.PointerPressed += ImageOnPointerPressed;
	}

	private void ImageOnPointerPressed(object? sender, PointerPressedEventArgs e)
	{
		if (ViewModel == null) return;
		// begin drawing
		this.GetParentWindow().PointerReleased += OnPointerReleased;
		this.GetParentWindow().PointerMoved += OnPointerMoved;
		Point position = e.GetPosition(Image);
		Point normalizedPosition = new(position.X / Image.Bounds.Width, position.Y / Image.Bounds.Height);
		ViewModel.BeginDrawing(normalizedPosition);
	}

	private void OnPointerMoved(object? sender, PointerEventArgs e)
	{
		if (ViewModel == null) throw new Exception();
		// update drawing
		Point position = e.GetPosition(Image);
		Point normalizedPosition = new(position.X / Image.Bounds.Width, position.Y / Image.Bounds.Height);
		ViewModel.UpdateDrawing(normalizedPosition);
	}

	private void OnPointerReleased(object? sender, PointerReleasedEventArgs e)
	{
		if (ViewModel == null) throw new Exception();
		this.GetParentWindow().PointerReleased -= OnPointerReleased;
		this.GetParentWindow().PointerMoved -= OnPointerMoved;
		// end drawing
		Point position = e.GetPosition(Image);
		Point normalizedPosition = new(position.X / Image.Bounds.Width, position.Y / Image.Bounds.Height);
		ViewModel.EndDrawing(normalizedPosition);
	}

	private void OnKeyDown(object? sender, KeyEventArgs e) => ViewModel?.NotifyKeyDown(e.Key);
}