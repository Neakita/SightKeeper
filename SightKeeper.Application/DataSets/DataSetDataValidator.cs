using FluentValidation;
using SightKeeper.Application.Extensions;

namespace SightKeeper.Application.DataSets;

public sealed class DataSetDataValidator : AbstractValidator<DataSetData>
{
	public DataSetDataValidator()
	{
		RuleFor(data => data.Name).NotEmpty();
		RuleFor(data => data.Resolution)
			.NotNull()
			.GreaterThan(0)
			.LessThanOrEqualTo(ushort.MaxValue)
			.MultiplierOf(32);
	}
}