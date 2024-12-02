using SightKeeper.Data.Binary.Model.Profiles.Modules;
using SightKeeper.Domain.Model.Profiles;

namespace SightKeeper.Data.Binary.Replication.Profiles.Modules;

internal class MultiModuleReplicator
{
	public MultiModuleReplicator(ReplicationSession session)
	{
		_classifierReplicator = new ClassifierModuleReplicator(session);
		_detectorReplicator = new DetectorModuleReplicator(session);
		_poser2DReplicator = new Poser2DModuleReplicator(session);
		_poser3DReplicator = new Poser3DModuleReplicator(session);
	}

	public void Replicate(
		Profile profile,
		IEnumerable<PackableModule> modules)
	{
		foreach (var module in modules)
			Replicate(profile, module);
	}

	private void Replicate(Profile profile, PackableModule module)
	{
		switch (module)
		{
			case PackableClassifierModule classifierModule:
				_classifierReplicator.Replicate(profile, classifierModule);
				return;
			case PackableDetectorModule detectorModule:
				_detectorReplicator.Replicate(profile, detectorModule);
				return;
			case PackablePoser2DModule poser2DModule:
				_poser2DReplicator.Replicate(profile, poser2DModule);
				return;
			case PackablePoser3DModule poser3DModule:
				_poser3DReplicator.Replicate(profile, poser3DModule);
				return;
			default:
				throw new ArgumentOutOfRangeException(nameof(module));
		}
	}

	private readonly ClassifierModuleReplicator _classifierReplicator;
	private readonly DetectorModuleReplicator _detectorReplicator;
	private readonly Poser2DModuleReplicator _poser2DReplicator;
	private readonly Poser3DModuleReplicator _poser3DReplicator;
}