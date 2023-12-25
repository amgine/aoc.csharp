using Spectre.Console;

namespace AoC.InputDownloader;

internal class Program
{
	interface IInputDownloader
	{
		Task DownloadAsync(int year, int day, string targetFileName);
	}

	class InputDownloader : IInputDownloader, IDisposable
	{
		public InputDownloader(string sessionCookie)
		{
			SessionCookie = FormatSessionCookie(sessionCookie);
			Http = new() { BaseAddress = new("https://adventofcode.com/") };
		}

		private HttpClient Http { get; }

		private string SessionCookie { get; }

		static string FormatSessionCookie(string sessionCookie)
		{
			if(!sessionCookie.StartsWith("session="))
			{
				sessionCookie = "session=" + sessionCookie;
			}
			return sessionCookie;
		}

		public async Task DownloadAsync(int year, int day, string targetFileName)
		{
			using var request  = new HttpRequestMessage(HttpMethod.Get, $"{year}/day/{day}/input");
			request.Headers.Add("Cookie", SessionCookie);
			using var response = await Http.SendAsync(request);
			response.EnsureSuccessStatusCode();
			using var fs = new FileStream(targetFileName, FileMode.Create, FileAccess.Write, FileShare.Read);
			await response.Content.CopyToAsync(fs);
			AnsiConsole.WriteLine($"Downloaded {targetFileName}");
		}

		public void Dispose() => Http.Dispose();
	}

	static async Task Main(string[] args)
	{
		var session = AnsiConsole.Ask<string>("Session cookie: ");

		var baseDirectory = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", ".."));
		using var downloader = new InputDownloader(session);
		await DownloadInputsAsync(downloader, baseDirectory);
	}

	static async Task DownloadInputsAsync(IInputDownloader downloader, string baseDirectory)
	{
		var now = DateTimeOffset.UtcNow;
		var maxYear = now.Month == 12 && now.Day < 25 ? now.Year - 1 : now.Year;
		for(int year = 2015; year <= maxYear; ++year)
		{
			await DownloadYearAsync(downloader, year, 25, Path.Combine(baseDirectory, year.ToString(), "inputs"));
		}
	}

	static async Task DownloadYearAsync(IInputDownloader downloader, int year, int days, string targetDirectory)
	{
		for(int day = 1; day <= 25; ++day)
		{
			var fileName = Path.Combine(targetDirectory, $"day{day:00}.txt");
			await downloader.DownloadAsync(year, day, fileName);
		}
	}
}
