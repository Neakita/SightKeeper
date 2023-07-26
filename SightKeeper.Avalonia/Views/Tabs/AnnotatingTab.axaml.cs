using Avalonia.ReactiveUI;
using SightKeeper.Avalonia.ViewModels.Tabs;

namespace SightKeeper.Avalonia.Views.Tabs;

public sealed partial class AnnotatingTab : ReactiveUserControl<AnnotatingViewModel>
{
	public AnnotatingTab()
	{
		InitializeComponent();
	}
	
	/*public AnnotatingTab(AnnotatingTabViewModel viewModel) : this()
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
		if (ViewModel == null) return;
		Point normalizedPosition = GetNormalizedPosition(e);
		if (!ViewModel.BeginDrawing(normalizedPosition)) return;
		Image.PointerPressed -= ImageOnPointerPressed;
		this.GetParentWindow().PointerReleased += OnPointerReleased;
		this.GetParentWindow().PointerMoved += OnPointerMoved;
	}

	private void OnPointerMoved(object? sender, PointerEventArgs e)
	{
		if (ViewModel == null) return;
		Point normalizedPosition = GetNormalizedPosition(e);
		ViewModel!.UpdateDrawing(normalizedPosition);
	}

	private void OnPointerReleased(object? sender, PointerReleasedEventArgs e)
	{
		if (ViewModel == null) return;
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
		if (ViewModel == null) return;
		Key key = e.Key;
		SetSelectedItemByIndex(key);

		if (key == Key.LeftCtrl) ViewModel!.ItemSelectionMode = true;
	}

	private void OnKeyUp(object? sender, KeyEventArgs e)
	{
		if (ViewModel == null) return;
		Key key = e.Key;
		if (key == Key.LeftCtrl) ViewModel!.ItemSelectionMode = false;
	}

	private void SetSelectedItemByIndex(Key key)
	{
		if (key is < Key.D1 or > Key.D9) return;
		int itemClassIndex = key - Key.D1;
		ViewModel!.SelectItemClassByIndex(itemClassIndex);
	}*/
}