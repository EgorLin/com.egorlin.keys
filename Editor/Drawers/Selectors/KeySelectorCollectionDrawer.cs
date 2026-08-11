using System.Collections.Generic;
using EgorLin.Keys.Collections.Data;
using EgorLin.Keys.Ids;
using EgorLin.Keys.Owners;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace EgorLin.Keys.Editor.Drawers.Selectors
{
    [CustomPropertyDrawer(typeof(KeySelectorCollectionBase), true)]
    public class KeySelectorCollectionDrawer : PropertyDrawer
    {
        private const float HeaderHeight   = 22f;
        private const float ToggleWidth    = 28f;
        private const float ToggleHeight   = 14f;
        private const float RowSpacing     = 2f;

        private readonly Dictionary<string, ReorderableList> _lists = new();
        private readonly Dictionary<string, SerializedProperty> _valuesProps = new();

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var propIsSpecific = property.FindPropertyRelative("isSpecificCollection");
            var propSpecific   = property.FindPropertyRelative("specificCollection");

            float y = position.y;

            // ─── Foldout + header ───────────────────────────────────────────
            var foldoutRect = new Rect(position.x, y, position.width, HeaderHeight);
            property.isExpanded = EditorGUI.Foldout(
                new Rect(foldoutRect.x, foldoutRect.y, 14f, foldoutRect.height),
                property.isExpanded, GUIContent.none, true);

            var labelRect = new Rect(foldoutRect.x + 14f, y, EditorGUIUtility.labelWidth - 14f, HeaderHeight);
            EditorGUI.LabelField(labelRect, label, EditorStyles.boldLabel);

            DrawDefaultCollectionRow(
                new Rect(labelRect.xMax + 4f, y, position.width - labelRect.width - 18f, HeaderHeight),
                propIsSpecific, propSpecific);

            y += HeaderHeight + RowSpacing;

            if (property.isExpanded)
            {
                var list = GetList(property);
                var listRect = new Rect(position.x, y, position.width, list.GetHeight());
                list.DoList(listRect);
            }

            EditorGUI.EndProperty();
        }

        // ─── Default-collection pill in the header ─────────────────────────

        private static void DrawDefaultCollectionRow(Rect rect,
            SerializedProperty propIsSpecific, SerializedProperty propSpecific)
        {
            var toggleRect = new Rect(
                rect.x,
                rect.y + (rect.height - ToggleHeight) / 2f,
                ToggleWidth, ToggleHeight);

            bool isOn = propIsSpecific.boolValue;
            EditorGUI.DrawRect(toggleRect, isOn
                ? new Color(0.25f, 0.5f, 0.85f, 0.8f)
                : new Color(0.3f, 0.3f, 0.3f, 0.6f));

            float thumbSize = toggleRect.height - 4f;
            float thumbX    = isOn ? toggleRect.xMax - thumbSize - 2f : toggleRect.x + 2f;
            EditorGUI.DrawRect(new Rect(thumbX, toggleRect.y + 2f, thumbSize, thumbSize),
                new Color(0.9f, 0.9f, 0.9f, 1f));

            var e = Event.current;
            if (e.type == EventType.MouseDown && e.button == 0 && toggleRect.Contains(e.mousePosition))
            {
                propIsSpecific.boolValue = !isOn;
                GUI.changed = true;
                e.Use();
            }

            var fieldRect = new Rect(toggleRect.xMax + 6f, rect.y, rect.width - ToggleWidth - 6f, rect.height);

            using (new EditorGUI.DisabledScope(!propIsSpecific.boolValue))
            {
                var newObj = EditorGUI.ObjectField(fieldRect, propSpecific.objectReferenceValue,
                    typeof(IKeyCollectionContainer), true);
                if (newObj != propSpecific.objectReferenceValue)
                    propSpecific.objectReferenceValue = newObj;
            }
        }

        // ─── ReorderableList setup ──────────────────────────────────────────

        private ReorderableList GetList(SerializedProperty collectionProperty)
        {
            var path = collectionProperty.propertyPath;

            var keysProp = collectionProperty.FindPropertyRelative("_keys");
            var valuesProp = keysProp.FindPropertyRelative("Values");

            if (_lists.TryGetValue(path, out var existing))
            {
                // Rebind to the current serializedObject/property each draw,
                // since SerializedProperty instances aren't safe to cache long-term.
                existing.serializedProperty = valuesProp;
                _valuesProps[path] = valuesProp;
                return existing;
            }

            var list = new ReorderableList(valuesProp.serializedObject, valuesProp,
                draggable: true, displayHeader: false, displayAddButton: true, displayRemoveButton: true);

            _valuesProps[path] = valuesProp;

            list.elementHeightCallback = index =>
            {
                var element = valuesProp.GetArrayElementAtIndex(index);
                return GetElementHeight(element);
            };

            list.drawElementCallback = (rect, index, active, focused) =>
            {
                rect.y += 2f;
                var element = valuesProp.GetArrayElementAtIndex(index);
                DrawElement(rect, element);
            };

            list.onAddCallback = l =>
            {
                int index = l.serializedProperty.arraySize;
                l.serializedProperty.arraySize++;
                var newElement = l.serializedProperty.GetArrayElementAtIndex(index);

                // Reset Key sub-fields to a clean state and seed the default
                // specific-collection from the parent collection's header setting.
                var keyProp = newElement.FindPropertyRelative("Key");
                var idProp = keyProp.FindPropertyRelative("id");
                var hashProp = idProp.FindPropertyRelative(nameof(KeyId.Hash));
                hashProp.intValue = KeyId.Empty.Hash;

                var entryIsSpecific = keyProp.FindPropertyRelative("isSpecificCollection");
                var entrySpecific   = keyProp.FindPropertyRelative("specificCollection");

                var collectionIsSpecific = collectionProperty.FindPropertyRelative("isSpecificCollection");
                var collectionSpecific   = collectionProperty.FindPropertyRelative("specificCollection");

                entryIsSpecific.boolValue = collectionIsSpecific.boolValue;
                entrySpecific.objectReferenceValue = collectionIsSpecific.boolValue
                    ? collectionSpecific.objectReferenceValue
                    : null;

                l.serializedProperty.serializedObject.ApplyModifiedProperties();
            };

            _lists[path] = list;
            return list;
        }

        private static float GetElementHeight(SerializedProperty element)
        {
            var keyProp = element.FindPropertyRelative("Key");
            var valueProp = element.FindPropertyRelative("Value");

            float keyHeight = EditorGUI.GetPropertyHeight(keyProp, true);
            float valueHeight = EditorGUI.GetPropertyHeight(valueProp, true) + 2f;

            return keyHeight + valueHeight + 4f;
        }

        private static void DrawElement(Rect rect, SerializedProperty element)
        {
            var keyProp = element.FindPropertyRelative("Key");
            var valueProp = element.FindPropertyRelative("Value");

            float keyHeight = EditorGUI.GetPropertyHeight(keyProp, true);
            var keyRect = new Rect(rect.x, rect.y, rect.width, keyHeight);
            EditorGUI.PropertyField(keyRect, keyProp, GUIContent.none, true);

            var valueRect = new Rect(
                rect.x, keyRect.yMax + 2f, rect.width,
                EditorGUI.GetPropertyHeight(valueProp, true));
            EditorGUI.PropertyField(valueRect, valueProp, new GUIContent(""), true);
        }

        // ─── Height ─────────────────────────────────────────────────────────

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float height = HeaderHeight + RowSpacing;

            if (!property.isExpanded)
                return height;

            var list = GetList(property);
            height += list.GetHeight();
            return height;
        }
    }
}
