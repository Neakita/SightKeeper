namespace SightKeeper.Application.DataSets.Creating;

public interface NewDataSetData : DataSetData
{
	DataSetType Type { get; }
}