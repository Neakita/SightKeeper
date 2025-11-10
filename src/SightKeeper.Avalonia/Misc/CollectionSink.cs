using System;
using System.Collections.Generic;
using Serilog.Core;
using Serilog.Events;

namespace SightKeeper.Avalonia.Misc;

internal sealed class CollectionSink(ICollection<string> collection) : ILogEventSink
{
	public int? MaximumEntries { get; set; }
	public Action<int, int>? RemoveRange { get; set; }

	public void Emit(LogEvent logEvent)
	{
		var message = logEvent.RenderMessage();
		collection.Add(message);
		if (MaximumEntries.HasValue && collection.Count > MaximumEntries && RemoveRange != null)
			RemoveRange(0, collection.Count - MaximumEntries.Value);
	}
}