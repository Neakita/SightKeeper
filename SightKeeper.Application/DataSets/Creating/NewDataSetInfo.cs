namespace SightKeeper.Application.DataSets.Creating;

public interface NewDataSetInfo : GeneralDataSetInfo
{
	int? Resolution { get; }
}