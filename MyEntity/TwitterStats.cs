namespace MyEntity
{
	public class TwitterStats
	{
		/// <summary>Total number of tweets received </summary>
		public long Count { get; set; }
		/// <summary>Average number of tweets received per minute </summary>
		public long Average { get; set; }
		/// <summary>The top <n> hashtags as comma delimited string </summary>
		public string TrendHashtags { get; set; }
	}
}
