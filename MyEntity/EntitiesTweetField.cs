using Newtonsoft.Json;
using System.Collections.Generic;

namespace MyEntity
{
	public class EntitiesTweetField
	{
		[JsonProperty("hashtags")]
		public List<HashtagTweetField> Hashtags { get; set; }
	}
}
