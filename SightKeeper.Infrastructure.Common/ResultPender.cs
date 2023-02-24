namespace SightKeeper.Infrastructure.Common;

public class ResultPender<TResult>
{
	private readonly TaskCompletionSource<TResult> _completionSource = new();

	public TResult GetResult() => _completionSource.Task.GetAwaiter().GetResult();

	public async Task<TResult> GetResultAsync(CancellationToken cancellationToken) =>
		await _completionSource.Task.WaitAsync(cancellationToken);

	public void SetResult(TResult value) => _completionSource.SetResult(value);
}