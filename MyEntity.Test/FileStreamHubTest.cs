using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Xunit;

namespace MyEntity.Test
{
	public class FileStreamHubTest
	{
		[Fact]
		public void SendTwitterStatsTest()
		{
			//arrange
			long counter = 1;
			var assembly = Assembly.GetExecutingAssembly();
			string resourceName = assembly.GetManifestResourceNames().Single(str => str.EndsWith("tweets.txt"));
			TwitterStatsServer statsServer = new TwitterStatsServer();
			TwitterStats stats = new TwitterStats();

			//act
			using (Stream stream = assembly.GetManifestResourceStream(resourceName))
			using (var reader = new StreamReader(stream, Encoding.UTF8))
			{
				while (!reader.EndOfStream)
				{
					var currentTweet = reader.ReadLine();

					var tweetData = JsonConvert.DeserializeObject<TweetData>(currentTweet);
					if (tweetData != null)
					{
						statsServer.AddHashtags(tweetData);
						stats.Count = counter;
						stats.TrendHashtags = statsServer.TrendHashtags.ToDelimitedString();
						counter++;
					}
				}

				reader.Close();
			}

			//assert
			Assert.True(stats.Count == counter-1);
			Assert.True(stats.TrendHashtags.Any());

		}
	}
}
