using System;
using System.Collections.Generic;
using System.Linq;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace SightKeeper.UI.WPF.ViewModels;

public sealed class HamburgerMenuVM : ReactiveObject
{
	[Reactive] public IEnumerable<HamburgerMenuItemVM> Items { get; set; }

	public HamburgerMenuItemVM? SelectedItem
	{
		get => _selectedItem;
		set
		{
			if (_selectedItem != null) _selectedItem.IsSelected = false;
			_selectedItem = value;
			if (_selectedItem != null) _selectedItem.IsSelected = true;
		}
	}


	public HamburgerMenuVM()
	{
		Items = Enumerable.Empty<HamburgerMenuItemVM>();
		this.WhenAnyValue(vm => vm.Items)
			.Subscribe(menuItems => { SelectedItem ??= menuItems.FirstOrDefault(); });
	}


	private HamburgerMenuItemVM? _selectedItem;
}