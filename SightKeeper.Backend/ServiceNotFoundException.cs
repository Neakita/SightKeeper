namespace SightKeeper.Backend;

public sealed class ServiceNotFoundException : Exception
{
	public Type ServiceType { get; }

	public ServiceNotFoundException(Type serviceType) : base($"Service of type {serviceType.FullName} not found.")
	{
		ServiceType = serviceType;
	}
}
