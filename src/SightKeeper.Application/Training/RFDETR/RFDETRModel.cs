namespace SightKeeper.Application.Training.RFDETR;

internal sealed class RFDETRModel
{
	public static RFDETRModel Nano => Create("Nano");
	public static RFDETRModel Small => Create("Small");
	public static RFDETRModel Medium => Create("Medium");
	public static RFDETRModel Large => Create("Large");

	private static RFDETRModel Create(string sizeDesignation)
	{
		return new RFDETRModel
		{
			Name = "RF-DETR-" + sizeDesignation[0],
			Argument = sizeDesignation.ToLower()
		};
	}

	public required string Name { get; init; }
	public required string Argument { get; init; }

	private RFDETRModel()
	{
	}
}