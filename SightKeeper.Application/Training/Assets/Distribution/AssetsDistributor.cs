using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using SightKeeper.Domain.DataSets.Assets;

namespace SightKeeper.Application.Training.Assets.Distribution;

public sealed class AssetsDistributor
{
	public IReadOnlyCollection<Asset>? Assets
	{
		get;
		set
		{
			InvalidateAssets();
			field = value;
		}
	}

	public double TrainFraction
	{
		get;
		set
		{
			InvalidateAssets();
			field = value;
		}
	} = 0.8;

	public double ValidationFraction
	{
		get;
		set
		{
			InvalidateAssets();
			field = value;
		}
	} = 0.15;

	public double TestFraction
	{
		get;
		set
		{
			InvalidateAssets();
			field = value;
		}
	} = 0.05;

	public IReadOnlyCollection<Asset> TrainAssets
	{
		get
		{
			if (_trainAssets == null)
				DistributeAssets();
			return _trainAssets;
		}
	}

	public IReadOnlyCollection<Asset> ValidationAssets
	{
		get
		{
			if (_validationAssets == null)
				DistributeAssets();
			return _validationAssets;
		}
	}

	public IReadOnlyCollection<Asset> TestAssets
	{
		get
		{
			if (_testAssets == null)
				DistributeAssets();
			return _testAssets;
		}
	}

	private IReadOnlyCollection<Asset>? _trainAssets;
	private IReadOnlyCollection<Asset>? _validationAssets;
	private IReadOnlyCollection<Asset>? _testAssets;

	private void InvalidateAssets()
	{
		_trainAssets = null;
		_validationAssets = null;
		_testAssets = null;
	}

	[MemberNotNull(nameof(_trainAssets), nameof(_validationAssets), nameof(_testAssets))]
	private void DistributeAssets()
	{
		if (Assets == null || Assets.Count == 0)
		{
			_trainAssets = _validationAssets = _testAssets = ReadOnlyCollection<Asset>.Empty;
			return;
		}

		var totalCount = Assets.Count;
		var fractionsSum = TrainFraction + ValidationFraction + TestFraction;
		var trainCount = (int)(totalCount * TrainFraction / fractionsSum);
		var validationCount = (int)(totalCount * ValidationFraction / fractionsSum);
		var testCount = totalCount - trainCount - validationCount;
		
		DistributionSession session = new(Assets);
		_trainAssets = session.PopAssets(AssetUsage.Train, trainCount);
		_validationAssets = session.PopAssets(AssetUsage.Validation, validationCount);
		_testAssets = session.PopAssets(AssetUsage.Test, testCount);
	}
}