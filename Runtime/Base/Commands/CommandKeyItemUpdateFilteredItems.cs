#if UNITY_EDITOR
using System.Collections.Generic;
using EgorLin.Keys.Base.Models;
using EgorLin.Keys.Utils;

namespace EgorLin.Keys.Base.Commands
{
	public static class CommandKeyItemUpdateFilteredItems
	{
        public static void Execute<T>(List<T> values, ModelKeyItems<T> model)
        {
            model.CleatFilteredItems();

            if (model.IsTextEmpty())
            {
                model.SetFilteredItems(values);

                return;
            }

            foreach (var value in values)
            {
                var tag = model.GetKeyItem(value);
                var has = SearchUtils.FuzzyMatch(tag.Value, model.Text);

                if (has)
                {
                    model.AddItem(value);
                }
            }
        }
	}
}
#endif
