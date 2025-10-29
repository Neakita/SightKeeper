using Autofac.Builder;

namespace SightKeeper.Application.Linux;

internal static class AutofacExtensions
{
	public static ParameterKeyFilterBuilder<TLimit, TReflectionActivatorData, TStyle> Parameter<TLimit, TReflectionActivatorData, TStyle>(
		this IRegistrationBuilder<TLimit, TReflectionActivatorData, TStyle> registration)
		where TReflectionActivatorData : ReflectionActivatorData
	{
		return new ParameterKeyFilterBuilder<TLimit, TReflectionActivatorData, TStyle>(registration);
	}
}