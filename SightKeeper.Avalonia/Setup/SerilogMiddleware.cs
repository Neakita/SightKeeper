using System;
using System.Linq;
using Autofac.Core;
using Autofac.Core.Resolving.Pipeline;
using CommunityToolkit.Diagnostics;
using Serilog;

namespace SightKeeper.Avalonia.Setup;

internal sealed class SerilogMiddleware : IResolveMiddleware
{
	public PipelinePhase Phase => PipelinePhase.ParameterSelection;

	public void Execute(ResolveRequestContext context, Action<ResolveRequestContext> next)
	{
		context.ChangeParameters(context.Parameters.Union(
			new[]
			{
				new ResolvedParameter(
					(p, _) => p.ParameterType == typeof(ILogger),
					(p, _) =>
					{
						Guard.IsNotNull(p.Member.DeclaringType);
						return Log.ForContext(p.Member.DeclaringType);
					})
			}));
		next(context);
	}
}