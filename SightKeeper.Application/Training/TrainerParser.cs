using System.Globalization;
using System.Reactive.Linq;
using System.Text.RegularExpressions;
using SightKeeper.Domain.Model.DataSets.Weights;

namespace SightKeeper.Application.Training;

public static class TrainerParser
{
    public static void Parse(
        IObservable<string> stream,
        out IObservable<TrainingProgress> trainingProgress) =>
        trainingProgress = stream.Where(content => TrainingProgressLineRegex.IsMatch(content)).Select(ParseTrainingProgress);

    private static readonly Regex TrainingProgressLineRegex = new(@" *\d+/\d+ *.*G *\d+(\.\d+) * \d+(\.\d+) * \d+(\.\d+) * \d* * \d*:.*", RegexOptions.Compiled);
    private static readonly Regex CurrentEpochRegex = new(@"(?<=^ *)\d+(?=/)", RegexOptions.Compiled);
    private static readonly Regex FloatingPointNumbersRegex = new(@"\d+(\.\d+)?", RegexOptions.Compiled);
    private static readonly NumberFormatInfo NumberFormatInfo = new() { NumberDecimalSeparator = "." };

    private static TrainingProgress ParseTrainingProgress(string content)
    {
        var currentEpoch = uint.Parse(CurrentEpochRegex.Match(content).Value);
        var floatingPointNumberMatches = FloatingPointNumbersRegex.Matches(content);
        var floatingPointNumbers = floatingPointNumberMatches
            .Skip(3)
            .Take(3)
            .Select(match => float.Parse(match.Value, NumberFormatInfo))
            .ToList();
        var boundingLoss = floatingPointNumbers[0];
        var classificationLoss = floatingPointNumbers[1];
        var deformationLoss = floatingPointNumbers[2];
        WeightsMetrics metrics = new(currentEpoch, boundingLoss, classificationLoss, deformationLoss);
        return new TrainingProgress(metrics);
    }
}