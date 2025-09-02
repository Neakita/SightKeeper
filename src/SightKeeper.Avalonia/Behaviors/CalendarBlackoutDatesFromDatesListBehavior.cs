using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Xaml.Interactivity;
using CommunityToolkit.Diagnostics;

namespace SightKeeper.Avalonia.Behaviors;

internal sealed class CalendarBlackoutDatesFromDatesListBehavior : Behavior<Calendar>
{
	public static readonly StyledProperty<IEnumerable<DateOnly>?> DatesProperty =
		AvaloniaProperty.Register<CalendarBlackoutDatesFromDatesListBehavior, IEnumerable<DateOnly>?>(nameof(Dates));

	public IEnumerable<DateOnly>? Dates
	{
		get => GetValue(DatesProperty);
		set => SetValue(DatesProperty, value);
	}

	protected override void OnAttached()
	{
		Guard.IsNotNull(AssociatedObject);
		AssociatedObject.BlackoutDates.Clear();
		if (Dates == null)
			return;
		foreach (var range in GetBlackoutRanges(Dates))
			AssociatedObject.BlackoutDates.Add(range);
	}

	protected override void OnDetaching()
	{
		Guard.IsNotNull(AssociatedObject);
		AssociatedObject.BlackoutDates.Clear();
	}

	protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
	{
		base.OnPropertyChanged(change);
		if (change.Property == DatesProperty)
			OnDatesChanged(change.GetOldValue<IEnumerable<DateOnly>?>());
	}

	private void OnDatesChanged(IEnumerable<DateOnly>? oldValue)
	{
		if (oldValue is INotifyCollectionChanged oldObservableCollection)
			oldObservableCollection.CollectionChanged -= OnDatesCollectionChanged;
		if (Dates is INotifyCollectionChanged newObservableCollection)
			newObservableCollection.CollectionChanged += OnDatesCollectionChanged;
		if (AssociatedObject == null)
			return;
		AssociatedObject.BlackoutDates.Clear();
		if (Dates == null)
			return;
		foreach (var range in GetBlackoutRanges(Dates))
			AssociatedObject.BlackoutDates.Add(range);
	}

	private static IEnumerable<CalendarDateRange> GetBlackoutRanges(IEnumerable<DateOnly> dates)
	{
		return InvertRanges(GetRanges(dates));
	}

	private static IEnumerable<CalendarDateRange> GetRanges(IEnumerable<DateOnly> dates)
	{
		using var enumerator = dates.GetEnumerator();
		if (!enumerator.MoveNext())
			yield break;
		var currentRangeStart = enumerator.Current;
		var currentRangeEnd = currentRangeStart;
		while (enumerator.MoveNext())
		{
			var current = enumerator.Current;
			if (current.DayNumber - currentRangeEnd.DayNumber != 1)
			{
				yield return new CalendarDateRange(
					currentRangeStart.ToDateTime(TimeOnly.MinValue),
					currentRangeEnd.ToDateTime(TimeOnly.MinValue));
				currentRangeStart = current;
				currentRangeEnd = current;
				continue;
			}
			currentRangeEnd = current;
		}
		yield return new CalendarDateRange(
			currentRangeStart.ToDateTime(TimeOnly.MinValue),
			currentRangeEnd.ToDateTime(TimeOnly.MinValue));
	}

	private static IEnumerable<CalendarDateRange> InvertRanges(IEnumerable<CalendarDateRange> ranges)
	{
		using var enumerator = ranges.GetEnumerator();
		if (!enumerator.MoveNext())
			yield break;
		var previousRange = enumerator.Current;
		while (enumerator.MoveNext())
		{
			var currentRange = enumerator.Current;
			yield return new CalendarDateRange(previousRange.End, currentRange.Start);
		}
	}

	private void OnDatesCollectionChanged(object? sender, NotifyCollectionChangedEventArgs args)
	{
		if (AssociatedObject == null)
			return;
		Guard.IsNotNull(sender);
		var datesCount = ((IReadOnlyCollection<DateOnly>)sender).Count;
		switch (args.Action)
		{
			case NotifyCollectionChangedAction.Add:
				Guard.IsEqualTo(args.NewStartingIndex, datesCount - 1); // Dates should not be inserted, only added at the end
				Guard.IsNotNull(args.NewItems);
				var date = args.NewItems.Cast<DateOnly>().Single();
				var lastRange = AssociatedObject.BlackoutDates[^1];
				var lastDate = DateOnly.FromDateTime(lastRange.End);
				Guard.IsGreaterThanOrEqualTo(date.DayNumber, lastDate.DayNumber);
				if (date.DayNumber == lastDate.DayNumber + 1)
				{
					AssociatedObject.BlackoutDates.RemoveAt(AssociatedObject.BlackoutDates.Count - 1);
					AssociatedObject.BlackoutDates.Add(new CalendarDateRange(lastRange.Start, date.ToDateTime(TimeOnly.MinValue)));
				}
				else AssociatedObject.BlackoutDates.Add(new CalendarDateRange(date.ToDateTime(TimeOnly.MinValue)));
				break;
			case NotifyCollectionChangedAction.Remove:
				Guard.IsNotNull(args.OldItems);
				var removedDate = args.OldItems.Cast<DateOnly>().Single();
				var (removedRange, index) = AssociatedObject.BlackoutDates
					.Select((range, index) => (range, index))
					.First(tuple => DateOnly.FromDateTime(tuple.range.Start).DayNumber >= removedDate.DayNumber);
				AssociatedObject.BlackoutDates.RemoveAt(index);
				var rangeStart = DateOnly.FromDateTime(removedRange.Start);
				var rangeEnd = DateOnly.FromDateTime(removedRange.End);
				if (rangeStart == rangeEnd)
					break;
				if (removedDate == rangeStart)
				{
					CalendarDateRange newRange = new(
						rangeStart.AddDays(1).ToDateTime(TimeOnly.MinValue),
						rangeEnd.ToDateTime(TimeOnly.MinValue));
					AssociatedObject.BlackoutDates.Insert(index, newRange);
				}
				else if (removedDate == rangeEnd)
				{
					CalendarDateRange newRange = new(
						rangeStart.ToDateTime(TimeOnly.MinValue),
						rangeEnd.AddDays(-1).ToDateTime(TimeOnly.MinValue));
					AssociatedObject.BlackoutDates.Insert(index, newRange);
				}
				else
				{
					CalendarDateRange firstRange = new(
						rangeStart.ToDateTime(TimeOnly.MinValue),
						removedDate.AddDays(-1).ToDateTime(TimeOnly.MinValue));
					CalendarDateRange secondRange = new(
						removedDate.AddDays(1).ToDateTime(TimeOnly.MinValue),
						rangeEnd.ToDateTime(TimeOnly.MinValue));
					AssociatedObject.BlackoutDates.Insert(index, firstRange);
					AssociatedObject.BlackoutDates.Insert(index + 1, secondRange);
				}
				break;
			case NotifyCollectionChangedAction.Replace:
				throw new NotSupportedException("");
			case NotifyCollectionChangedAction.Move:
				throw new NotSupportedException("");
			case NotifyCollectionChangedAction.Reset:
				AssociatedObject.BlackoutDates.Clear();
				Guard.IsNotNull(Dates);
				foreach (var range in GetBlackoutRanges(Dates))
					AssociatedObject.BlackoutDates.Add(range);
				break;
			default:
				throw new ArgumentOutOfRangeException();
		}
	}
}