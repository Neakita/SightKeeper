using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using CommunityToolkit.Mvvm.ComponentModel;
using SightKeeper.Avalonia.Dialogs;

namespace SightKeeper.Avalonia;

public sealed partial class MainViewModel : ViewModel, DialogHost
{
	public DialogManager DialogManager { get; }
	public IReadOnlyCollection<TabItemViewModel> Tabs { get; }
	[ObservableProperty] public partial TabItemViewModel SelectedTab { get; set; }
	[ObservableProperty] public partial object? Content { get; private set; }

	public MainViewModel(
		DialogManager dialogManager,
		IReadOnlyCollection<TabItemViewModel> tabs)
	{
		DialogManager = dialogManager;
		Tabs = tabs;
		SelectedTab = Tabs.First();
	}

	private IDisposable _contentDisposable = Disposable.Empty;

	partial void OnSelectedTabChanged(TabItemViewModel value)
	{
		_contentDisposable.Dispose();
		(Content, _contentDisposable) = value.ContentFactory();
	}
}