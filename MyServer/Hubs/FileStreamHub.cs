using Microsoft.AspNetCore.SignalR;
using MyEntity;
using MyEntity.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyServer.Hubs
{
	public class FileStreamHub : Hub
	{
		public async IAsyncEnumerable<TwitterStats> SendTwitterStats([EnumeratorCancellation] CancellationToken cancellationToken = default)
		{
			Stopwatch stopwatch = new Stopwatch();
			long counter = 1;
			var assembly = Assembly.GetExecutingAssembly();
			string resourceName = assembly.GetManifestResourceNames().Single(str => str.EndsWith("tweets.txt"));
			TwitterStatsServer statsServer = new TwitterStatsServer();
			TwitterStats stats = new TwitterStats();
			using (Stream stream = assembly.GetManifestResourceStream(resourceName))
			using (var reader = new StreamReader(stream, Encoding.UTF8))
			{
				stopwatch.Start();
				while (!reader.EndOfStream)
				{
					cancellationToken.ThrowIfCancellationRequested();
					var currentTweet = await reader.ReadLineAsync();

					var tweetData = JsonConvert.DeserializeObject<TweetData>(currentTweet);
					if (tweetData != null)
					{
						statsServer.AddHashtags(tweetData);

						stats.Count = counter;
						stats.Average = statsServer.GetAveragePerMinute(counter, stopwatch);
						stats.TrendHashtags = statsServer.TrendHashtags.ToDelimitedString();

						yield return stats;
						await Task.Delay(200, cancellationToken);
						counter++;
					}
				}

				reader.Close();
			}

			stopwatch.Stop();
		}

		/// <summary>
		/// Populates file with <n> number of rows from read from Twitter stream
		/// </summary>
		/// <param name="rowCount">The number of rows to write to file</param>
		/// <returns></returns>
		private async Task PopulateTwitterSampleTweetsFile(int rowCount)
		{
			string path = @"c:\temp\tweets.txt";
			if (File.Exists(path))
			{
				File.Delete(path);
			}

			FileStream f = new FileStream(path, FileMode.OpenOrCreate);
			StreamWriter s = new StreamWriter(f);
			using (HttpClient httpClient = new HttpClient())
			{
				httpClient.Timeout = TimeSpan.FromMilliseconds(Timeout.Infinite);
				httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TwitterConnection.BearerToken);

				using (var stream = await httpClient.GetStreamAsync(TwitterConnection.TwitterTweetsUrl))
				{
					long counter = 1;
					using (var reader = new StreamReader(stream, Encoding.UTF8))
					{
						while (!reader.EndOfStream && counter <= rowCount)
						{
							var currentTweet = await reader.ReadLineAsync();
							s.WriteLine(currentTweet);
							counter++;

						}

					}
				}
			}

			s.Close();
			f.Close();
		}
	}
}
