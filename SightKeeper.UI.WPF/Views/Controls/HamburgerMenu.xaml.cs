using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media.Animation;
using SightKeeper.UI.WPF.ViewModels;

namespace SightKeeper.UI.WPF.Views.Controls;

public partial class HamburgerMenu
{
	public static readonly DependencyProperty CollapsedWidthProperty;
	public static readonly DependencyProperty ExpandedWidthProperty;
	public static readonly DependencyProperty AnimationDurationProperty;
	public static readonly DependencyProperty ItemsSourceProperty;


	static HamburgerMenu()
	{
		CollapsedWidthProperty = DependencyProperty.Register("CollapsedWidth", typeof(double), typeof(HamburgerMenu),
			new FrameworkPropertyMetadata(50, FrameworkPropertyMetadataOptions.NotDataBindable));
		
		ExpandedWidthProperty = DependencyProperty.Register("ExpandedWidth", typeof(double), typeof(HamburgerMenu),
			new FrameworkPropertyMetadata(150, FrameworkPropertyMetadataOptions.NotDataBindable));
		
		AnimationDurationProperty = DependencyProperty.Register("AnimationDuration", typeof(ushort), typeof(HamburgerMenu),
			new FrameworkPropertyMetadata(200, FrameworkPropertyMetadataOptions.NotDataBindable));
		
		ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(IEnumerable<HamburgerMenuItemVM>),
			typeof(HamburgerMenu),
			new FrameworkPropertyMetadata(Enumerable.Empty<HamburgerMenuItemVM>(),
				FrameworkPropertyMetadataOptions.None, ItemsSourcePropertyChangedCallback));
	}
	
	private static void ItemsSourcePropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e) =>
		((HamburgerMenuVM)((HamburgerMenu) d).DataContext).Items = (IEnumerable<HamburgerMenuItemVM>) e.NewValue;


	public HamburgerMenu()
	{
		InitializeComponent();
		SideMenu.Width = CollapsedWidth;
		ToggleExpandButtonRow.Height = new GridLength(CollapsedWidth);
		ToggleExpandButton.Checked += (_, _) => Expand();
		ToggleExpandButton.Unchecked += (_, _) => Collapse();
	}


	private double CollapsedWidth => (double) GetValue(CollapsedWidthProperty);
	private double ExpandedWidth => (double) GetValue(ExpandedWidthProperty);
	private Duration AnimationDuration => new(TimeSpan.FromMilliseconds((double) GetValue(AnimationDurationProperty)));

	private readonly EasingFunctionBase _easingFunction = new PowerEase {Power = 2, EasingMode = EasingMode.EaseInOut};


	private void Collapse() => SideMenu.BeginAnimation(WidthProperty, CreateAnimation(CollapsedWidth));

	private void Expand() => SideMenu.BeginAnimation(WidthProperty, CreateAnimation(ExpandedWidth));

	private DoubleAnimation CreateAnimation(double to) =>
		new(SideMenu.Width, to, AnimationDuration, FillBehavior.HoldEnd) {EasingFunction = _easingFunction};
}