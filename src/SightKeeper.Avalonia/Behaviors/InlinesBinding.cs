using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Documents;
using Avalonia.Threading;
using Avalonia.Xaml.Interactivity;
using CommunityToolkit.Diagnostics;

namespace SightKeeper.Avalonia.Behaviors;

internal sealed class InlinesBinding : Behavior<TextBlock>
{
	public static readonly StyledProperty<IEnumerable<string>?> InlinesProperty =
		AvaloniaProperty.Register<InlinesBinding, IEnumerable<string>?>(nameof(Inlines));

	public IEnumerable<string>? Inlines
	{
		get => GetValue(InlinesProperty);
		set => SetValue(InlinesProperty, value);
	}

	protected override void OnAttached()
	{
		Subscribe(Inlines);
	}

	protected override void OnDetaching()
	{
		Unsubscribe(Inlines);
	}

	[SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
	protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
	{
		base.OnPropertyChanged(change);
		if (AssociatedObject == null)
			return;
		if (change.Property != InlinesProperty)
			return;
		var (oldValue, newValue) = change.GetOldAndNewValue<IEnumerable<string>?>();
		Unsubscribe(oldValue);
		Subscribe(newValue);
		AssociatedObject.Inlines = new InlineCollection();
		if (newValue == null)
			return;
		var runs = newValue.Select(str => new Run(str + '\n'));
		AssociatedObject.Inlines.AddRange(runs);
	}

	private void Subscribe(IEnumerable<string>? newValue)
	{
		if (newValue is INotifyCollectionChanged notifyCollectionChanged)
			notifyCollectionChanged.CollectionChanged += OnInlinesCollectionChanged;
	}

	private void Unsubscribe(IEnumerable<string>? oldValue)
	{
		if (oldValue is INotifyCollectionChanged notifyCollectionChanged)
			notifyCollectionChanged.CollectionChanged -= OnInlinesCollectionChanged;
	}

	private void OnInlinesCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
	{
		Dispatcher.UIThread.Invoke(() =>
		{
			Guard.IsNotNull(AssociatedObject);
			Guard.IsNotNull(AssociatedObject.Inlines);
			if (e.OldItems != null)
				AssociatedObject.Inlines.RemoveRange(e.OldStartingIndex, e.OldItems.Count);
			if (e.NewItems == null)
				return;
			var runs = e.NewItems.Cast<string>().Select(str => new Run(str + '\n'));
			AssociatedObject.Inlines.InsertRange(e.NewStartingIndex, runs);
		});
	}
}