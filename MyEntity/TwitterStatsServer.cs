using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace MyEntity
{
	public class TwitterStatsServer
	{
		private const int maxListSize = 10000;
		private const int trendSize = 10;

		public TwitterStatsServer()
		{
			AllHashtags = new List<string>();
			TrendHashtags = new List<string>();
		}

		public List<string> AllHashtags { get; set; }

		public IEnumerable<string> TrendHashtags
		{
			get
			{
				if (AllHashtags != null && AllHashtags.Count > 0)
				{
					return AllHashtags.GetMostFrequent(trendSize);
				}

				return Enumerable.Empty<string>();

			}
			set { }
		}

		public void AddHashtags(TweetData tweetData)
		{
			if (tweetData.TweetHashtags != null)
			{
				if (maxListSize < AllHashtags.Count + tweetData.TweetHashtags.Count())
				{
					for (int i = 0; i < AllHashtags.Count + tweetData.TweetHashtags.Count - maxListSize; i++)
					{
						AllHashtags.RemoveAt(0);
					}
				}

				foreach (var hashtag in tweetData.TweetHashtags)
				{
					AllHashtags.Add(hashtag);
				}
			}
		}

		public long GetAveragePerMinute(long counter, Stopwatch stopwatch)
		{
			return stopwatch.ElapsedMilliseconds > 0 ? counter * 60000 / stopwatch.ElapsedMilliseconds : 0;
		}
	}
}
