using System.Collections.Immutable;
using System.Numerics;
using FluentAssertions;
using NSubstitute;
using SightKeeper.Application.Prediction;
using SightKeeper.Application.Prediction.Handling.MouseMoving;
using SightKeeper.Application.Prediction.Handling.MouseMoving.Decorators;

namespace SightKeeper.Services.Tests;

public sealed class DetectionMouseMoverAccumulationDecoratorTests
{
	[Theory]
	[InlineData(0, 0)]
	[InlineData(1.3, 0.3)]
	[InlineData(-1.3, -0.3)]
	[InlineData(1.7, -0.3)]
	[InlineData(-1.7, 0.3)]
	public void ShouldHaveProperAccumulation(float move, float expectedAccumulation)
	{
		AccumulationDecorator accumulationDecorator = new(Substitute.For<DetectionMouseMover>());
		accumulationDecorator.Move(EmptyMouseMovingContext, new Vector2(move, 0));
		accumulationDecorator.Accumulation.X.Should().BeApproximately(expectedAccumulation, 0.001f);
	}

	[Theory]
	[InlineData(3, 0, new[] {1.4f, 1.6f})]
	[InlineData(4, -0.2f, new[] {1.4f, 2.4f,})]
	public void ShouldMoveTotalBy(float totalExpectedMove, float expectedAccumulation, IEnumerable<float> moves)
	{
		var mouseMover = Substitute.For<DetectionMouseMover>();
		AccumulationDecorator accumulationDecorator = new(mouseMover);
		var totalMove = 0f;
		mouseMover.Move(Arg.Any<MouseMovingContext>(), Arg.Do<Vector2>(v => totalMove += v.X));
		foreach (var move in moves)
			accumulationDecorator.Move(EmptyMouseMovingContext, new Vector2(move, 0));
		accumulationDecorator.Accumulation.X.Should().BeApproximately(expectedAccumulation, 0.001f);
		totalMove.Should().BeApproximately(totalExpectedMove, 0.001f);
	}

	private static MouseMovingContext EmptyMouseMovingContext => new(new DetectionData(Array.Empty<byte>(), ImmutableList<DetectionItem>.Empty), new DetectionItem());
}