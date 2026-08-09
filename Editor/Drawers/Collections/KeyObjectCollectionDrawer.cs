using System.Collections.Generic;
using EgorLin.Keys.Backend;
using EgorLin.Keys.Base.Commands;
using EgorLin.Keys.Base.Models;
using EgorLin.Keys.Collections.Data;
using EgorLin.Keys.Editor.CodeGeneration;
using EgorLin.Keys.Editor.Drawers.Utils;
using EgorLin.Keys.Editor.Widgets.Base;
using EgorLin.Keys.Editor.Widgets.Dialogs;
using EgorLin.Keys.Editor.Widgets.Items;
using EgorLin.Keys.Editor.Widgets.Paths;
using EgorLin.Keys.Editor.Widgets.Windows;
using EgorLin.Keys.Tags.Data;
using EgorLin.Pools;
using UnityEditor;
using UnityEngine;

namespace EgorLin.Keys.Editor.Drawers.Collections
{
    [CustomPropertyDrawer(typeof(KeyObjectCollection<>), useForChildren: true)]
    public class KeyObjectCollectionDrawer : PropertyDrawer
    {
        private readonly Dictionary<int, DrawerState> _stateMap = new();
 
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            DrawLayout(property);
        }
 
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
            => 0f;
 
        private DrawerState GetState(SerializedProperty property)
        {
            var id = property.serializedObject.GetHashCode();
 
            if (!_stateMap.TryGetValue(id, out var state))
            {
                state = new DrawerState();
                _stateMap[id] = state;
            }
 
            return state;
        }
 
        private void DrawLayout(SerializedProperty property)
        {
            var state   = GetState(property);
 
            var target  = property.serializedObject.targetObject;
            var fi      = ReflectionUtils.GetFieldInfo(target.GetType(), property.propertyPath);
            var rawColl = fi?.GetValue(target);
 
            if (rawColl == null)
            {
                EditorGUILayout.HelpBox("Could not resolve KeyObjectCollection.", MessageType.Error);
                return;
            }
 
            DrawTyped(property, rawColl, state);
        }
 
        private void DrawTyped(SerializedProperty property, object rawColl, DrawerState state)
        {
            KeyWidgetInfoBox.Draw();
 
            if (KeyWidgetSaveButton.DrawSaveButton(state.IsSaveDirty))
            {
                var owner = property.serializedObject.targetObject;
                CommandKeyCollectionSaveAsset.Execute(owner);
 
                EditorApplication.delayCall += () =>
                {
                    state.IsSaveDirty = false;
                    AssetDatabase.Refresh();
                    KeysBackend.Rebuild();
                };
            }
 
            if (KeyWidgetGenerateButton.DrawButton())
            {
                KeyCollectionCodeGenerator.Generate();
            }
 
            var collBase = (KeyCollectionBase)rawColl;
 
            KeyWidgetPathRoot.Draw(collBase, state.ModelPath, () => SetDirty(state));
 
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
 
            var adapter = state.GetAdapter(rawColl);
 
            if (KeyWidgetItemHeader.Draw(adapter.Count))
            {
                ClearViaAdapter(adapter, state);
            }
 
            KeyWidgetBase.DrawSpaceSmall();
            DrawSearchBar(adapter, state);
            KeyWidgetBase.DrawSpaceSmall();
 
            if (KeyWidgetItemAddButton.Draw())
            {
                OpenAddViaAdapter(adapter, collBase, state);
            }
 
            KeyWidgetBase.DrawSpaceSmall();
 
            if (adapter.ModelIsDirty)
            {
                adapter.RebuildFilteredItems();
                adapter.SetModelDirty(false);
            }
 
            DrawListViaAdapter(property, adapter, state);
 
            EditorGUILayout.EndVertical();
        }
 
        private static void DrawSearchBar(ICollectionAdapter adapter, DrawerState state)
        {
            var textSearch = KeyWidgetItemSearchBar.DrawSearchBar(adapter.ModelText);
 
            if (textSearch != adapter.ModelText)
            {
                SetDirty(state);
            }
 
            adapter.SetModelText(textSearch);
        }
 
        private static void DrawListViaAdapter(
            SerializedProperty property,
            ICollectionAdapter adapter,
            DrawerState state)
        {
            if (adapter.Count == 0)
            {
                KeyWidgetItemList.DrawEmptyHelpBox(false, adapter.ModelText);
                return;
            }
 
            var valuesArraySP = property
                .FindPropertyRelative("_keys")
                ?.FindPropertyRelative("Values");
 
            var result = adapter.DrawList(valuesArraySP);
 
            if (adapter.ConsumeValueDirty())
            {
                SetDirty(state);
            }
 
            if (result.HasItemToRemove)
            {
                adapter.RemoveAt(result.Index, state);
            }
 
            if (result.HasItemToRename)
            {
                adapter.RenameAt(result.Index, state);
            }
        }
 
        private static void ClearViaAdapter(ICollectionAdapter adapter, DrawerState state)
        {
            if (!KeyWidgetDialogClear.Draw(adapter.Count))
            {
                return;
            }
 
            adapter.Clear(state);
        }
 
        private static void OpenAddViaAdapter(
            ICollectionAdapter adapter,
            KeyCollectionBase collBase,
            DrawerState state)
        {
            var lockedTagIds = PoolFastList<string>.Spawn();
 
            foreach (var key in collBase.GetAllKeys())
            {
                lockedTagIds.Add(key.Value);
            }
 
            KeyWidgetWindowAddTag.Open(lockedTagIds, true, tagId =>
            {
                adapter.Add(tagId, state);
                PoolFastList<string>.Recycle(lockedTagIds);
            });
        }
 
        private static void SetDirty(DrawerState state)
        {
            state.SetModelDirty(true);
            state.IsSaveDirty = true;
        }
 
        private interface ICollectionAdapter
        {
            int    Count         { get; }
            string ModelText     { get; }
            bool   ModelIsDirty  { get; }
 
            void SetModelText(string text);
            void SetModelDirty(bool value);
            void RebuildFilteredItems();
 
            KeyWidgetItemListResult DrawList(SerializedProperty valuesArraySP);
            bool ConsumeValueDirty();
 
            void RemoveAt(int filteredIndex, DrawerState state);
            void RenameAt(int filteredIndex, DrawerState state);
            void Add(string tag, DrawerState state);
            void Clear(DrawerState state);
        }
 
        private sealed class CollectionAdapter<T> : ICollectionAdapter
        {
            private readonly KeyObjectCollection<T> _collection;
            private readonly ModelKeyItems<KeyObjectEntry<T>> _model;
 
            public CollectionAdapter(
                KeyObjectCollection<T> collection,
                ModelKeyItems<KeyObjectEntry<T>> model)
            {
                _collection = collection;
                _model      = model;
            }
 
            public int    Count        => _collection.Keys.Count;
            public string ModelText    => _model.Text;
            public bool   ModelIsDirty => _model.IsDirty;
 
            public void SetModelText(string text)      => _model.SetTextSearch(text);
            public void SetModelDirty(bool value)      => _model.SetDirty(value);
 
            public void RebuildFilteredItems()
            {
                CommandKeyItemUpdateFilteredItems.Execute(_collection.Keys, _model);
            }
 
            public KeyWidgetItemListResult DrawList(SerializedProperty valuesArraySP)
            {
                return KeyWidgetItemList.DrawList(_model, entry =>
                {
                    SerializedProperty valueSP = null;
 
                    if (valuesArraySP != null)
                    {
                        var srcIndex = _collection.Keys.IndexOf(entry);
 
                        if (srcIndex >= 0 && srcIndex < valuesArraySP.arraySize)
                        {
                            valueSP = valuesArraySP
                                .GetArrayElementAtIndex(srcIndex)
                                .FindPropertyRelative("Value");
                        }
                    }
 
                    EditorGUI.BeginChangeCheck();
 
                    var raw = KeyWidgetItemRawObject.Draw(entry, valueSP);
 
                    if (EditorGUI.EndChangeCheck())
                    {
                        _valueDirty = true;
                    }
 
                    return raw;
                });
            }
 
            public bool ConsumeValueDirty()
            {
                var dirty   = _valueDirty;
                _valueDirty = false;
                return dirty;
            }
 
            private bool _valueDirty;
 
            public void RemoveAt(int filteredIndex, DrawerState state)
            {
                var entry = _model.FilteredItems[filteredIndex];
                _collection.RemoveEntry(entry);
                SetDirty(state);
            }
 
            public void RenameAt(int filteredIndex, DrawerState state)
            {
                var entry        = _model.FilteredItems[filteredIndex];
                var lockedTagIds = PoolFastList<string>.Spawn();
 
                foreach (var key in _collection.GetAllKeys())
                {
                    lockedTagIds.Add(key.Value);
                }
 
                KeyWidgetWindowAddTag.Open(lockedTagIds, true, tag =>
                {
                    for (int i = 0; i < _collection.Keys.Count; i++)
                    {
                        var k = _collection.Keys[i];
 
                        if (k.Key.Id == entry.Key.Id)
                        {
                            entry.Key.Value      = tag;
                            _collection.Keys[i]  = entry;
                            break;
                        }
                    }
 
                    SetDirty(state);
                    PoolFastList<string>.Recycle(lockedTagIds);
                });
            }
 
            public void Add(string tag, DrawerState state)
            {
                _collection.AddEntry(tag);
                SetDirty(state);
            }
 
            public void Clear(DrawerState state)
            {
                _collection.ClearEntries();
                SetDirty(state);
            }
 
            private static void SetDirty(DrawerState state)
            {
                state.SetModelDirty(true);
                state.IsSaveDirty = true;
            }
        }
 
        private sealed class DrawerState
        {
            public readonly ModelKeyWidgetPathRoot ModelPath = new();
            public bool IsSaveDirty;
 
            private ICollectionAdapter _adapter;
            private object             _lastCollection;
 
            public ICollectionAdapter GetAdapter(object rawColl)
            {
                if (_adapter != null && ReferenceEquals(_lastCollection, rawColl))
                {
                    return _adapter;
                }
 
                _adapter        = CreateAdapter(rawColl);
                _lastCollection = rawColl;
                return _adapter;
            }
 
            public void SetModelDirty(bool value) => _adapter?.SetModelDirty(value);
 
            private static ICollectionAdapter CreateAdapter(object rawColl)
            {
                var collType    = rawColl.GetType();
                var typeArg     = collType.GetGenericArguments()[0];
 
                var modelType   = typeof(ModelKeyItems<>)
                    .MakeGenericType(typeof(KeyObjectEntry<>).MakeGenericType(typeArg));
 
                var keyExtractorType = typeof(System.Func<,>)
                    .MakeGenericType(
                        typeof(KeyObjectEntry<>).MakeGenericType(typeArg),
                        typeof(KeyTag));
 
                var entryParam   = System.Linq.Expressions.Expression.Parameter(
                    typeof(KeyObjectEntry<>).MakeGenericType(typeArg), "entry");
                var keyField     = System.Linq.Expressions.Expression.Field(entryParam, "Key");
                var lambda       = System.Linq.Expressions.Expression.Lambda(
                    keyExtractorType, keyField, entryParam);
                var keyExtractor = lambda.Compile();
 
                var model = System.Activator.CreateInstance(modelType, keyExtractor);
 
                var adapterType = typeof(CollectionAdapter<>).MakeGenericType(typeArg);
                return (ICollectionAdapter)System.Activator.CreateInstance(
                    adapterType, rawColl, model);
            }
        }
    }
}