using SightKeeper.Application.Training.Parsing;

namespace SightKeeper.Application.Tests;

public class DarknetDetectorParserTests
{
    private const string Sample = "4: 1292.006348, 1291.511230 avg loss, 0.000000 rate, 43.140034 seconds, 256 images";
    
    [Fact]
    public void ShouldProperlyParse()
    {
        TrainingProgress expectedProgress = new(4, 1291.511230);
        DarknetDetectorOutputParser parser = new();
        var result = parser.TryParse(Sample, out var progress);
        result.Should().BeTrue();
        progress!.Value.Should().BeEquivalentTo(expectedProgress);
    }

    [Theory]
    [InlineData("Region 94 Avg IOU: -nan(ind), Class: -nan(ind), Obj: -nan(ind), No Obj: 0.505853, .5R: -nan(ind), .75R: -nan(ind), count: 0")]
    public void ShouldNotParseWithoutAnyExceptions(string sample)
    {
        DarknetDetectorOutputParser parser = new();
        var result = parser.TryParse(sample, out var progress);
        result.Should().BeFalse();
        progress.Should().BeNull();
    }
}