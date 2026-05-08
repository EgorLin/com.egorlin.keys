#if UNITY_EDITOR
using EgorLin.Keys.Backend.Indexers.Tags;
using EgorLin.Keys.Utils;

namespace EgorLin.Keys.Tags.Commands
{
	public static  class CommandKeyTagHas
	{
		public static bool Has(string value)
		{
			var format = KeyTagUtils.Format(value);
			return KeyTagIndexer.Has(format);
		}
	}
}
#endif