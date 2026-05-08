#if UNITY_EDITOR
using System.Collections.Generic;
using EgorLin.Keys.Items.Data;
using EgorLin.Keys.Tags.Commands;

namespace EgorLin.Keys.Base.Models
{
	public struct ModelKeyCollectionEntrySearch
	{
		public List<KeyItem> Paths;
		public List<KeyItem> Keys;
		
		public string GetFullPath()
		{
			var path = "";
			for (var index = 0; index < Paths.Count; index++)
			{
				var pathValue = Paths[index];
				var keyTag = CommandKeyTagGetTag.Execute(pathValue.TagId);

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
