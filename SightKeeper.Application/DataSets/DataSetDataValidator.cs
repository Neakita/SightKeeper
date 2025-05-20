using FluentValidation;

namespace SightKeeper.Application.DataSets;

public sealed class DataSetDataValidator : AbstractValidator<DataSetData>
{
	public static DataSetDataValidator Instance { get; } = new();

	private DataSetDataValidator()
	{
		RuleFor(data => data.Name).NotEmpty();
	}
}