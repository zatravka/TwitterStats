using Newtonsoft.Json;

namespace MyEntity
{
	public class HashtagTweetField
	{
		[JsonProperty("tag")]
		public string Tag { get; set; }

		[JsonProperty("start")]
		public int Start { get; set; }

		[JsonProperty("end")]
		public int End { get; set; }
	}
}
