using FluentValidation;

namespace SightKeeper.Application.DataSets;

public sealed class DataSetDataValidator : AbstractValidator<DataSetData>
{
	public DataSetDataValidator()
	{
		RuleFor(data => data.Name).NotEmpty();
	}
}