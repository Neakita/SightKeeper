using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text.RegularExpressions;
using Serilog;
using Serilog.Events;
using SerilogTimings;
using SightKeeper.Domain.Model.Detector;

namespace SightKeeper.Application.Training.Parsing;

public class DarknetDetectorOutputParser : DarknetOutputParser<DetectorModel>
{
    public bool TryParse(string output, [NotNullWhen(true)] out TrainingProgress? progress)
    {
        using var operation = Operation.At(LogEventLevel.Verbose).Begin("Parsing darknet output \"{Output}\"", output);
        progress = null;
        if (!TargetStringRegex.IsMatch(output))
        {
            Log.Verbose("Could not parse output: {Output} as it is not in expected format", output);
            operation.Complete();
            return false;
        }
        var currentBatch = uint.Parse(ExtractSingleSubstringWithRegex(output, CurrentBatchRegex));
        var averageLoss = double.Parse(ExtractSingleSubstringWithRegex(output, AverageLossRegex), CultureInfo.InvariantCulture);
        progress = new TrainingProgress(currentBatch, averageLoss);
        operation.Complete(nameof(progress), progress);
        return true;
    }

    /// <summary>
    /// Matches the whole string "4: 1292.006348, 1291.511230 avg loss, 0.000000 rate, 43.140034 seconds, 256 images"
    /// </summary>
    private static readonly Regex TargetStringRegex = new("\\d+: \\d+.\\d+, \\d+.\\d+ avg loss, \\d+.\\d+ rate, \\d+.\\d+ seconds, \\d+ images", RegexOptions.Compiled);

    /// <summary>
    /// Matches only the "4" at start of the "4: 1292.006348, 1291.511230 avg loss, 0.000000 rate, 43.140034 seconds, 256 images"
    /// </summary>
    private static readonly Regex CurrentBatchRegex = new("^\\d+");

    /// <summary>
    /// Matches only the 1291.511230 (third number) of the "4: 1292.006348, 1291.511230 avg loss, 0.000000 rate, 43.140034 seconds, 256 images"
    /// </summary>
    private static readonly Regex AverageLossRegex = new(@"\d+.\d+(?= avg loss)", RegexOptions.Compiled);

    private static string ExtractSingleSubstringWithRegex(string str, Regex regex)
    {
        var matches = regex.Matches(str);
        if (matches.Count == 0)
            throw new DarknetOutputParsingException($"No matches found in \"{str}\" using regex \"{regex}\"");
        if (matches.Count > 1)
            throw new DarknetOutputParsingException($"Multiple matches found in \"{str}\", using regex \"{regex}\" but only one is expected");
        return matches[0].Value;
    }
}