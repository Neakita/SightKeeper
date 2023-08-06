using System.Reactive.Disposables;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.ReactiveUI;
using CommunityToolkit.Diagnostics;
using ReactiveUI;
using SightKeeper.Avalonia.Extensions;
using SightKeeper.Avalonia.ViewModels.Annotating;

namespace SightKeeper.Avalonia.Views.Annotating;

public sealed partial class AnnotatingTab : ReactiveUserControl<AnnotatorViewModel>
{
	public AnnotatingTab()
	{
		this.WhenActivated(OnActivated);
		InitializeComponent();
	}

	private TopLevel? _topLevel;

	private void OnActivated(CompositeDisposable disposable)
	{
		Disposable.Create(OnDeactivated).DisposeWith(disposable);
		_topLevel = this.GetTopLevel();
		_topLevel.PointerWheelChanged += OnTopLevelScrolled;
	}

	private void OnTopLevelScrolled(object? sender, PointerWheelEventArgs e)
	{
		ScrollItemClass(e.Delta.Y);
	}

	private void ScrollItemClass(double delta)
	{
		if (ViewModel?.Tools == null || delta == 0)
			return;
		ViewModel.Tools.ScrollItemClass(delta > 0);
	}

	private void OnDeactivated()
	{
		Guard.IsNotNull(_topLevel);
		_topLevel.PointerWheelChanged -= OnTopLevelScrolled;
	}
}