namespace SightKeeper.Application.Training.DFINE;

public sealed class DFineModel
{
	public static DFineModel Nano { get; } = new("dfine_hgnetv2_n_custom.yml");
	public static DFineModel Small { get; } = new("dfine_hgnetv2_s_custom.yml");
	public static DFineModel Medium { get; } = new("dfine_hgnetv2_m_custom.yml");
	public static DFineModel Large { get; } = new("dfine_hgnetv2_l_custom.yml");
	public static DFineModel ExtraLarge { get; } = new("dfine_hgnetv2_x_custom.yml");

	public string ConfigName { get; }

	private DFineModel(string configName)
	{
		ConfigName = configName;
	}
}