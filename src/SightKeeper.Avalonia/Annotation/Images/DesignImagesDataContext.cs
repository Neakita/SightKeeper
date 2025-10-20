using System;
using System.Collections.Generic;
using System.Linq;

namespace SightKeeper.Avalonia.Annotation.Images;

internal sealed class DesignImagesDataContext : ImagesDataContext
{
	private static IReadOnlyList<string> SampleImageFileNames { get; } =
	[
		"kfSample1.jpg",
		"kfSample2.jpg",
		"kfSample3.jpg",
		"kfSample4.jpg",
		"kfSample5.jpg",
		"kfSample6.jpg",
		"kfSample7.jpg",
		"kfSample8.jpg"
	];

	private static IEnumerable<AnnotationImageDataContext> GetImages()
	{
		return GetTimestamps().Select((creationTimestamp, index) =>
			new DesignAnnotationImageDataContext(SampleImageFileNames[index % SampleImageFileNames.Count], creationTimestamp, index < 4));
	}

	private static IEnumerable<DateTimeOffset> GetTimestamps()
	{
		var now = DateTimeOffset.Now;
		yield return now.AddYears(-3);
		yield return now.AddYears(-2);
		yield return now.AddYears(-1);
		yield return now.AddMonths(-3);
		yield return now.AddMonths(-2);
		yield return now.AddMonths(-1);
		yield return now.AddDays(-3);
		yield return now.AddDays(-2);
		yield return now.AddDays(-1);
		yield return now;
	}

	public IReadOnlyCollection<AnnotationImageDataContext> Images => GetImages().ToList();

	public int SelectedImageIndex { get; set; }
}