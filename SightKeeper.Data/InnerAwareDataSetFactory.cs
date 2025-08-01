using SightKeeper.Application.DataSets.Creating;

namespace SightKeeper.Data;

public interface InnerAwareDataSetFactory<TDataSet> : DataSetFactory<TDataSet>
{
	new (TDataSet wrappedSet, TDataSet innerSet) CreateDataSet();

	TDataSet DataSetFactory<TDataSet>.CreateDataSet()
	{
		var sets = CreateDataSet();
		return sets.wrappedSet;
	}
}