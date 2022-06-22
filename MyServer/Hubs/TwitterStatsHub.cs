using Microsoft.AspNetCore.SignalR;
using MyEntity;
using MyEntity.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyServer.Hubs
{
	public class TwitterStatsHub : Hub
	{
		public async IAsyncEnumerable<TwitterStats> SendTwitterStats([EnumeratorCancellation] CancellationToken cancellationToken = default)
		{
			using (HttpClient httpClient = new HttpClient())
			{
				httpClient.Timeout = TimeSpan.FromMilliseconds(Timeout.Infinite);
				httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TwitterConnection.BearerToken);

				using (var stream = await httpClient.GetStreamAsync(TwitterConnection.TwitterTweetsUrl))
				{
					Stopwatch stopwatch = new Stopwatch();
					long counter = 1;
					TwitterStatsServer statsServer = new TwitterStatsServer();
					TwitterStats stats = new TwitterStats();
					using (var reader = new StreamReader(stream, Encoding.UTF8))
					{
						stopwatch.Start();
						while (!reader.EndOfStream && cancellationToken.IsCancellationRequested == false)
						{
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
				}
			}
		}


	}
}
