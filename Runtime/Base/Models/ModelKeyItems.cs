using System;
using System.Collections.Generic;
using EgorLin.Keys.Tags.Data;
using UnityEngine;

namespace EgorLin.Keys.Base.Models
{
	public class ModelKeyItems<T>
	{
		private readonly Func<T, KeyTag> _getKeyTags;
		
		public string Text { get; private set; } = string.Empty;
        public List<T> FilteredItems { get; } = new();
        public Vector2 ScrollPosition { get; private set; }
        public bool IsDirty { get; private set; } = true;

        public ModelKeyItems(Func<T, KeyTag> getKeyTags)
        {
	        _getKeyTags = getKeyTags;
        }

        public void SetTextSearch(string textSearch)
        {
            Text = textSearch;
        }

        public bool IsTextEmpty()
        {
	        return string.IsNullOrEmpty(Text);
        }
        
        public void CleatFilteredItems()
        {
	        FilteredItems.Clear();
        }

        public void SetFilteredItems(List<T> items)
        {
	        foreach (var item in items)
	        {
		        FilteredItems.Add(item);
	        }
        }

        public void AddItem(T value)
        {
	        FilteredItems.Add(value);
        }

        public void SetScrollPosition(Vector2 scroll)
        {
	        ScrollPosition = scroll;
        }

        public KeyTag GetKeyItem(T item)
        {
	        return _getKeyTags.Invoke(item);
        }

        public void SetDirty(bool value)
        {
	        IsDirty = value;
        }
	}
}