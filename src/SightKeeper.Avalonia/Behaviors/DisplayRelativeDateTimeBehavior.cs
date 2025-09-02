using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using System.Timers;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Threading;
using Avalonia.Xaml.Interactivity;
using CommunityToolkit.Diagnostics;
using Humanizer;
using Serilog;
using Timer = System.Timers.Timer;

namespace SightKeeper.Avalonia.Behaviors;

internal sealed class DisplayRelativeDateTimeBehavior : Behavior<TextBlock>
{
	public static readonly StyledProperty<DateTimeOffset> DateTimeProperty =
		AvaloniaProperty.Register<DisplayRelativeDateTimeBehavior, DateTimeOffset>(nameof(DateTime));

	private static readonly HashSet<DisplayRelativeDateTimeBehavior> ActiveBehaviors = new();
	private static Timer? _timer;
	private static readonly ILogger Logger = Log.ForContext<DisplayRelativeDateTimeBehavior>();

	public DateTimeOffset DateTime
	{
		get => GetValue(DateTimeProperty);
		set => SetValue(DateTimeProperty, value);
	}

	protected override void OnAttachedToVisualTree()
	{
		UpdateText();
		if (_timer == null)
		{
			_timer = new Timer(TimeSpan.FromSeconds(1));
			_timer.Start();
		}
		_timer.Elapsed += OnTimerElapsed;
		bool isAdded = ActiveBehaviors.Add(this);
		Guard.IsTrue(isAdded);
	}

	protected override void OnDetachedFromVisualTree()
	{
		Guard.IsNotNull(_timer);
		_timer.Elapsed -= OnTimerElapsed;
		bool isRemoved = ActiveBehaviors.Remove(this);
		Guard.IsTrue(isRemoved);
		if (ActiveBehaviors.Count != 0)
			return;
		_timer.Stop();
		_timer.Dispose();
		_timer = null;
	}

	private void OnTimerElapsed(object? sender, ElapsedEventArgs e)
	{
		try
		{
			Dispatcher.UIThread.Invoke(UpdateText);
		}
		catch (TaskCanceledException exception)
		{
			Logger.Verbose(exception,
				"An exception was thrown from the dispatcher. " +
				"The task was probably canceled due to exiting the application, which is ok.");
		}
	}

	private void UpdateText()
	{
		if (AssociatedObject != null)
			AssociatedObject.Text = DateTime.Humanize(culture: CultureInfo.InvariantCulture);
	}
}