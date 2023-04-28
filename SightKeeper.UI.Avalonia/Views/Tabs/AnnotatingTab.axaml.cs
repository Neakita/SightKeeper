using Avalonia;
using Avalonia.Input;
using Avalonia.ReactiveUI;
using SightKeeper.Common;
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
		KeyUp += OnKeyUp;
	}

	protected override void OnInitialized()
	{
		base.OnInitialized();
		Image.PointerPressed += ImageOnPointerPressed;
	}

	private void ImageOnPointerPressed(object? sender, PointerPressedEventArgs e)
	{
		ViewModel.ThrowIfNull(nameof(ViewModel));
		Point normalizedPosition = GetNormalizedPosition(e);
		if (!ViewModel!.BeginDrawing(normalizedPosition)) return;
		Image.PointerPressed -= ImageOnPointerPressed;
		this.GetParentWindow().PointerReleased += OnPointerReleased;
		this.GetParentWindow().PointerMoved += OnPointerMoved;
	}

	private void OnPointerMoved(object? sender, PointerEventArgs e)
	{
		ViewModel.ThrowIfNull(nameof(ViewModel));
		Point normalizedPosition = GetNormalizedPosition(e);
		ViewModel!.UpdateDrawing(normalizedPosition);
	}

	private void OnPointerReleased(object? sender, PointerReleasedEventArgs e)
	{
		ViewModel.ThrowIfNull(nameof(ViewModel));
		this.GetParentWindow().PointerReleased -= OnPointerReleased;
		this.GetParentWindow().PointerMoved -= OnPointerMoved;
		Image.PointerPressed += ImageOnPointerPressed;
		Point normalizedPosition = GetNormalizedPosition(e);
		ViewModel!.EndDrawing(normalizedPosition);
	}

	private Point GetNormalizedPosition(PointerEventArgs e)
	{
		Point position = e.GetPosition(Image);
		Point normalizedPosition = new(position.X / Image.Bounds.Width, position.Y / Image.Bounds.Height);
		return normalizedPosition;
	}

	private void OnKeyDown(object? sender, KeyEventArgs e)
	{
		ViewModel.ThrowIfNull(nameof(ViewModel));
		Key key = e.Key;
		SetSelectedItemByIndex(key);

		if (key == Key.LeftCtrl) ViewModel!.ItemSelectionMode = true;
	}

	private void OnKeyUp(object? sender, KeyEventArgs e)
	{
		ViewModel.ThrowIfNull(nameof(ViewModel));
		Key key = e.Key;
		if (key == Key.LeftCtrl) ViewModel!.ItemSelectionMode = false;
	}

	private void SetSelectedItemByIndex(Key key)
	{
		if (key is < Key.D1 or > Key.D9) return;
		int itemClassIndex = key - Key.D1;
		ViewModel!.SelectItemClassByIndex(itemClassIndex);
	}
}