using SightKeeper.Data.Binary.Model.Profiles.Modules;
using SightKeeper.Domain.Model.Profiles;
using SightKeeper.Domain.Model.Profiles.Modules;

namespace SightKeeper.Data.Binary.Replication.Profiles.Modules;

internal static class MultiModuleReplicator
{
	public static void Replicate(
		Profile profile,
		IEnumerable<PackableModule> modules,
		ReplicationSession session)
	{
		foreach (var module in modules)
			Replicate(profile, module, session);
	}

	private static Module Replicate(Profile profile, PackableModule module, ReplicationSession session) => module switch
	{
		PackableClassifierModule classifierModule => _classifierReplicator.Replicate(profile, classifierModule, session),
		PackableDetectorModule detectorModule => _detectorReplicator.Replicate(profile, detectorModule, session),
		PackablePoser2DModule poser2DModule => _poser2DReplicator.Replicate(profile, poser2DModule, session),
		PackablePoser3DModule poser3DModule => _poser3DReplicator.Replicate(profile, poser3DModule, session),
		_ => throw new ArgumentOutOfRangeException(nameof(module))
	};

	private static readonly ClassifierModuleReplicator _classifierReplicator = new();
	private static readonly DetectorModuleReplicator _detectorReplicator = new();
	private static readonly Poser2DModuleReplicator _poser2DReplicator = new();
	private static readonly Poser3DModuleReplicator _poser3DReplicator = new();
}