using System;
using System.Linq;
using Autofac;
using Autofac.Core;
using Autofac.Core.Resolving.Pipeline;
using CommunityToolkit.Diagnostics;
using Serilog;

namespace SightKeeper.Avalonia.Infrastructure;

internal sealed class SerilogMiddleware : IResolveMiddleware
{
	public PipelinePhase Phase => PipelinePhase.ParameterSelection;

	public void Execute(ResolveRequestContext requestContext, Action<ResolveRequestContext> next)
	{
		requestContext.ChangeParameters(requestContext.Parameters.Append(
			new ResolvedParameter(
				(p, _) => p.ParameterType == typeof(ILogger),
				(p, context) =>
				{
					Guard.IsNotNull(p.Member.DeclaringType);
					return context.Resolve<ILogger>().ForContext(p.Member.DeclaringType);
				})
		));
		next(requestContext);
	}
}