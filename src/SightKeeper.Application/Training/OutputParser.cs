using SightKeeper.Application.Training.RFDETR;

namespace SightKeeper.Application.Training;

internal interface OutputParser
{
	IObservable<EpochResult> Parse(IObservable<string> output);
}