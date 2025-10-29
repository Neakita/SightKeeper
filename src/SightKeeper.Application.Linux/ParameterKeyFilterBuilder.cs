using System.Reflection;
using Autofac;
using Autofac.Builder;

namespace SightKeeper.Application.Linux;

internal sealed class ParameterKeyFilterBuilder<TLimit, TReflectionActivatorData, TStyle>
	where TReflectionActivatorData : ReflectionActivatorData
{
	public ParameterKeyFilterBuilder(IRegistrationBuilder<TLimit, TReflectionActivatorData, TStyle> registration)
	{
		_registration = registration;
	}

	public IRegistrationBuilder<TLimit, TReflectionActivatorData, TStyle> KeyFilter<TParameter>(object key)
		where TParameter : notnull
	{
		return _registration.WithParameter(ParameterSelector, ValueProvider);

		bool ParameterSelector(ParameterInfo parameterInfo, IComponentContext _)
		{
			return parameterInfo.ParameterType == typeof(TParameter);
		}

		object? ValueProvider(ParameterInfo _, IComponentContext context)
		{
			return context.ResolveKeyed<TParameter>(key);
		}
	}

	private readonly IRegistrationBuilder<TLimit, TReflectionActivatorData, TStyle> _registration;
}