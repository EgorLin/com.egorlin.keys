#if UNITY_EDITOR
using System.Collections.Generic;
using EgorLin.Keys.Tags.Data;

namespace EgorLin.Keys.Base.Models
{
	public struct ModelKeyCollectionEntrySearch
	{
		public List<KeyTag> Paths;
		public List<KeyTag> Keys;
		
		public string GetFullPath()
		{
			var path = "";
			for (var index = 0; index < Paths.Count; index++)
			{
				var keyTag = Paths[index];

				path += keyTag.Value;

				if (index != Paths.Count - 1)
				{
					path += " / ";
				}
			}

			return path;
		}
	}
}
#endif
