using System;
using System.Collections.Generic;
using System.Linq;

namespace MyEntity
{
	public static class HashtagTrendHelper
	{
		public static IEnumerable<T> GetMostFrequent<T>(this IEnumerable<T> inputs, int topXMostFrequent)
		{
			var uniqueGroups = inputs.GroupBy(i => i);

			if (uniqueGroups.Count() <= topXMostFrequent)
			{
				return uniqueGroups.Select(group => group.Key).ToList();
			}

			return uniqueGroups.OrderByDescending(i => i.Count())
							   .Take(topXMostFrequent)
							   .Select(group => group.Key).ToList();
		}

		public static string ToDelimitedString<T>(this IEnumerable<T> inputs)
		{
			string delimiter = ",";
			if (inputs != null)
			{
				return String.Join(delimiter, inputs.Select(s => s));
			}

			return string.Empty;
		}
	}
}
