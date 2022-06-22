
using Newtonsoft.Json;

namespace MyEntity
{
	public class DataTweetField
	{
		[JsonProperty("id")]
		public string Id { get; set; }

		[JsonProperty("text")]
		public string Text { get; set; }

		[JsonProperty("entities")]
		public EntitiesTweetField Entities { get; set; }
}
}
