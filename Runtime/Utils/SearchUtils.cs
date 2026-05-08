namespace EgorLin.Keys.Utils
{
	public static class SearchUtils
	{
		public static bool FuzzyMatch(string text, string pattern)
		{
			var indexPattern = 0;

			for (int indexText = 0; indexText < text.Length && indexPattern < pattern.Length; indexText++)
			{
				if (text[indexText] == pattern[indexPattern])
				{
					indexPattern++;
				}
			}

			return indexPattern == pattern.Length;
		}
	}
}
