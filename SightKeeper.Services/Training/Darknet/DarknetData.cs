namespace SightKeeper.Services.Training.Darknet;

public sealed class DarknetData
{
	public byte ClassesCount { get; set; }
	public string ImagesListPath { get; set; } = string.Empty;
	public string ClassesListPath { get; set; } = string.Empty;
	public string OutputPath { get; set; } = string.Empty;
	public byte Top { get; set; } = 2;

	public override string ToString() => 
		$@"classes={ClassesCount}
train={ImagesListPath}
labels = {ClassesListPath}
backup = {OutputPath}
top={Top}";
}