using SightKeeper.Domain.Model.Detector;

namespace SightKeeper.Application.Training;

public sealed class DetectorTrainer : ModelTrainer<DetectorModel>
{
	public bool IsRunning { get; private set; }
	public float? Progress { get; private set; }
	public uint? CurrentBatch { get; private set; }
	public uint? MaxBatches { get; private set; }
	public string? Status { get; private set; }
	public double? AverageLoss { get; private set; }
	public TimeSpan? TimeRemaining { get; private set; }
	public bool CanEndTraining { get; private set; }

	public void BeginTraining(DetectorModel model, bool fromScratch)
	{
		CanEndTraining = false;
		IsRunning = true;
		throw new NotImplementedException();
	}

	public void EndTraining()
	{
		IsRunning = false;
		Progress = null;
		CurrentBatch = null;
		MaxBatches = null;
		Status = null;
		AverageLoss = null;
		TimeRemaining = null;
		CanEndTraining = true;
		throw new NotImplementedException();
	}
}
