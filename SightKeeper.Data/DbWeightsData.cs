using SightKeeper.Domain.Model.DataSets;

namespace SightKeeper.Data;

internal sealed class DbWeightsData
{
	public enum DataFormat
	{
		ONNX,
		PT
	}
	
	public WeightsData Data { get; }
	public Weights Weights { get; }
	public DataFormat Format { get; }

	public DbWeightsData(WeightsData data, Weights weights, DataFormat format)
	{
		Data = data;
		Weights = weights;
		Format = format;
	}

	// ReSharper disable once UnusedMember.Local
	// EF constructor
	private DbWeightsData()
	{
		Data = null!;
		Weights = null!;
	}
}