using System.Diagnostics;
using System.Reactive.Linq;
using System.Text.RegularExpressions;
using CommunityToolkit.Diagnostics;

namespace SightKeeper.Application.Training;

public static class CondaVirtualEnvironment
{
	public static async Task ActivateOrCreateAsync(SessionCommandRunner commandRunner, string directoryPath, string? pythonVersion = null, CancellationToken cancellationToken = default)
	{
		string condaPath = await GetCondaPathAsync(cancellationToken);
		commandRunner.ExecuteCommand($"eval \"$({condaPath} shell.bash hook)\"");
		if (await TryActivateAsync(commandRunner, directoryPath))
		{
			if (pythonVersion != null)
				await EnsurePythonVersion(commandRunner, pythonVersion);
			return;
		}

		await CreateAsync(commandRunner, directoryPath, pythonVersion);
		await ActivateAsync(commandRunner, directoryPath);
		if (pythonVersion != null)
			await EnsurePythonVersion(commandRunner, pythonVersion);
	}

	public static async Task CreateAsync(SessionCommandRunner commandRunner, string directoryPath, string? pythonVersion)
	{
		var createCommand = $"conda create --prefix {directoryPath} --yes";
		if (pythonVersion != null)
			createCommand += $" python={pythonVersion}";
		createCommand += "\n";
		commandRunner.ExecuteCommand(createCommand);
		await commandRunner.Output.FirstAsync(output => output == "# To activate this environment, use");
	}

	public static async Task<bool> TryActivateAsync(SessionCommandRunner commandRunner, string directoryPath)
	{
		commandRunner.ExecuteCommand($"conda activate {directoryPath}");
		var activeEnvironmentName = await QueryInfoAsync(commandRunner, "active env location");
		return activeEnvironmentName == directoryPath;
	}

	public static async Task ActivateAsync(SessionCommandRunner commandRunner, string directoryPath)
	{
		bool isActivated = await TryActivateAsync(commandRunner, directoryPath);
		Guard.IsTrue(isActivated);
	}

	private static async Task<string> GetCondaPathAsync(CancellationToken cancellationToken)
	{
		// Try to find conda in common locations
		string[] possiblePaths =
		[
			"~/miniconda3/bin/conda",
			"~/anaconda3/bin/conda",
			"/opt/conda/bin/conda",
			"conda"
		];
        
		foreach (var path in possiblePaths)
		{
			try
			{
				var process = new Process
				{
					StartInfo = new ProcessStartInfo
					{
						FileName = "/bin/bash",
						Arguments = $"-c \"{path} --version\"",
						RedirectStandardOutput = true,
						RedirectStandardError = true,
						UseShellExecute = false,
						CreateNoWindow = true,
					}
				};
                
				process.Start();
				await process.WaitForExitAsync(cancellationToken);
                
				if (process.ExitCode == 0)
				{
					return path;
				}
			}
			catch
			{
				// ignored
			}
		}
        
		throw new Exception("Conda not found. Please ensure conda is installed.");
	}

	private static async Task<string> QueryInfoAsync(SessionCommandRunner commandRunner, string valueName)
	{
		commandRunner.ExecuteCommand("conda info");
		var output = await commandRunner.Output.FirstAsync(output => output.TrimStart().StartsWith(valueName));
		var value = output.Split(" : ").Last();
		return value;
	}

	private static async Task EnsurePythonVersion(SessionCommandRunner commandRunner, string expectedVersion)
	{
		var regex = new Regex(@"^Python \d+\.\d+\.\d+$");
		commandRunner.ExecuteCommand("python --version");
		var actualVersion = await commandRunner.Output.FirstAsync(output => regex.IsMatch(output));
		actualVersion = actualVersion.Replace("Python ", string.Empty);
		Guard.IsEqualTo(actualVersion, expectedVersion);
	}
}