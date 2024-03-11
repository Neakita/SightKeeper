namespace SightKeeper.Application.DataSets.Creating;

public interface NewDataSetInfo : DataSetInfo
{
	int? Resolution { get; }
}