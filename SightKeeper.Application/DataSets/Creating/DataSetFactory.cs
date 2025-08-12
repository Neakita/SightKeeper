namespace SightKeeper.Application.DataSets.Creating;

public interface DataSetFactory<out TDataSet>
{
	TDataSet CreateDataSet();
}