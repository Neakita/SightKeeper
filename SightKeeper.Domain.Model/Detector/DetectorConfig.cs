using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Domain.Model.Detector;

public sealed class DetectorConfig : ModelConfig
{
	public const string BatchPlaceholder = "{Batch}";
	public const string SubdivisionsPlaceholder = "{Subdivisions}";
	public const string WidthPlaceholder = "{Width}";
	public const string HeightPlaceholder = "{Height}";
	public const string MaxBatchesPlaceholder = "{MaxBatches}";
	public const string Steps80Placeholder = "{Steps80}";
	public const string Steps90Placeholder = "{Steps90}";
	public const string ClassesCountPlaceholder = "{ClassesCount}";
	public const string FiltersPlaceholder = "{Filters}";
	public const string GaussianFiltersPlaceholder = "{GaussianFilters}";

	public DetectorConfig(string name, string content) : base(name, content)
	{
		Validate(content);
	}

	private static void Validate(string content)
	{
		string[] requiredFields =
		{
			BatchPlaceholder, SubdivisionsPlaceholder, WidthPlaceholder, HeightPlaceholder, MaxBatchesPlaceholder,
			Steps80Placeholder, Steps90Placeholder, ClassesCountPlaceholder
		};
		List<string> missingFields = requiredFields.Where(requiredField =>
			!content.Contains(requiredField, StringComparison.InvariantCultureIgnoreCase)).ToList();
		if (missingFields.Any())
			throw new Exception($"Detector config is missing required fields: {string.Join(", ", missingFields)}");
	}
}
