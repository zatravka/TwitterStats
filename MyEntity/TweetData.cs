using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace MyEntity
{
	public class TweetData
	{
		public List<string> TweetHashtags
		{
			get
			{
				if (DataTweetField != null && DataTweetField.Entities != null
					&& DataTweetField.Entities.Hashtags != null)
				{
					return DataTweetField.Entities.Hashtags.Select(s => s.Tag).ToList();
				}

				return null;
			}
		}

		[JsonProperty("data")]
		public DataTweetField DataTweetField { get; set; }
	}
}
