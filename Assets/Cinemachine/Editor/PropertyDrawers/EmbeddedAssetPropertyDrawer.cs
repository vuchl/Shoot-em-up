using UnityEngine;
using UnityEditor;
using System;
using Cinemachine.Utility;
using System.Linq;

namespace Cinemachine.Editor
{
    [CustomPropertyDrawer(typeof(CinemachineEmbeddedAssetPropertyAttribute))]
    internal sealed class EmbeddedAssetPropertyDrawer : PropertyDrawer
    {
        const float vSpace = 2;
        const float kIndentAmount = 15;
        const float kBoxMargin = 3;
        float HelpBoxHeight { get { return EditorGUIUtility.singleLineHeight * 2.5f; } }
        bool mExpanded = false;

        bool WarnIfNull 
        {
            get
            {
                var attr = attribute as CinemachineEmbeddedAssetPropertyAttribute;
                return attr == null ? false : attr.WarnIfNull;
            }
        }

        float HeaderHeight { get { return EditorGUIUtility.singleLineHeight * 1.5f; } }
        float DrawHeader(Rect rect, string text)
        {
            float delta = HeaderHeight - EditorGUIUtility.singleLineHeight;
            rect.y += delta; rect.height -= delta;
            EditorGUI.LabelField(rect, new GUIContent(text), EditorStyles.boldLabel);
            return HeaderHeight;
        }

        string HeaderText(SerializedProperty property)
        {
            var attrs = property.serializedObject.targetObject.GetType()
                .GetCustomAttributes(typeof(HeaderAttribute), false);
            if (attrs != null && attrs.Length > 0)
                return ((HeaderAttribute)attrs[0]).header;
            return null;
        }

        bool AssetHasCustomEditor(SerializedProperty property)
        {
            ScriptableObject asset = property.objectReferenceValue as ScriptableObject;
            if (asset != null)
                return ActiveEditorTracker.HasCustomEditor(asset);
            return false;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            bool hasCustomEditor = AssetHasCustomEditor(property);
            float height = base.GetPropertyHeight(property, label);
            height += + 2 * (kBoxMargin + vSpace);
            if (mExpanded && !hasCustomEditor)
            {
                height += HelpBoxHeight + kBoxMargin;
                ScriptableObject asset = property.objectReferenceValue as ScriptableObject;
                if (asset != null)
                {
                    SerializedObject so = new SerializedObject(asset);
                    var prop = so.GetIterator();
                    prop.NextVisible(true);
                    do
                    {
                        if (prop.name == "m_Script")
                            continue;
                        string header = HeaderText(prop);
                        if (header != null)
                            height += HeaderHeight + vSpace;
                        height += EditorGUI.GetPropertyHeight(prop, false) + vSpace;
                    }
                    while (prop.NextVisible(prop.isExpanded));
                    height += kBoxMargin;
                }
            }
            return height;
        }

        public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
        {
            bool hasCustomEditor = AssetHasCustomEditor(property);
            rect.y += vSpace; rect.height -= 2 * vSpace;
            GUI.Box(rect, GUIContent.none, GUI.skin.box);
            rect.y += kBoxMargin;

            rect.height = EditorGUIUtility.singleLineHeight;
            EditorGUIUtility.labelWidth -= kBoxMargin;
            AssetFieldWithCreateButton(
                new Rect(rect.x + kBoxMargin, rect.y, rect.width - 2 * kBoxMargin, rect.height),
                property, WarnIfNull,
                property.serializedObject.targetObject.name + " " + label.text);

            ScriptableObject asset = property.objectReferenceValue as ScriptableObject;
            if (asset != null && !hasCustomEditor)
            {
                mExpanded = EditorGUI.Foldout(rect, mExpanded, GUIContent.none);
                if (mExpanded)
                {
                    rect.y += rect.height + kBoxMargin + vSpace;
                    rect.x += kIndentAmount + kBoxMargin;
                    rect.width -= kIndentAmount + 2 * kBoxMargin;
                    EditorGUIUtility.labelWidth -= kIndentAmount;

                    EditorGUI.HelpBox(
                        new Rect(rect.x, rect.y, rect.width, HelpBoxHeight),
                        "This is a shared asset.\n"
                            + "Changes made here will apply to all users of this asset", 
                        MessageType.Info);

                    rect.y += HelpBoxHeight + kBoxMargin;
                    SerializedObject so = new SerializedObject(property.objectReferenceValue);
                    var prop = so.GetIterator();
                    prop.NextVisible(true);

                    var indent = EditorGUI.indentLevel;
                    do
                    {
                        if (prop.name == "m_Script")
                            continue;
                        string header = HeaderText(prop);
                        if (header != null)
                        {
                            DrawHeader(rect, header);
                            rect.y += HeaderHeight + vSpace;
                            rect.height -= HeaderHeight + vSpace;
                        }
                        rect.height = EditorGUI.GetPropertyHeight(prop, false);
                        EditorGUI.indentLevel = indent + prop.depth;
                        EditorGUI.PropertyField(rect, prop);
                        rect.y += rect.height + vSpace;
                    } while (prop.NextVisible(prop.isExpanded));

                    if (GUI.changed)
                        so.ApplyModifiedProperties();
                }
                EditorGUIUtility.labelWidth += kIndentAmount;
            }
            EditorGUIUtility.labelWidth += kBoxMargin;
        }


        Type[] mAssetTypes = null;

        static Type EmbeddedAssetType(SerializedProperty property)
        {
            Type type = property.serializedObject.targetObject.GetType();
            var a = property.propertyPath.Split('.');
            for (int i = 0; i < a.Length; ++i)
                type = type.GetField(a[i]).FieldType;
            return type;
        }

        void AssetFieldWithCreateButton(
            Rect r, SerializedProperty property, bool warnIfNull, string defaultName)
        {
            // Collect all the eligible asset types
            Type type = EmbeddedAssetType(property);
            if (mAssetTypes == null)
                mAssetTypes = ReflectionHelpers.GetTypesInAllLoadedAssemblies(
                    (Type t) => t.IsSubclassOf(type)).ToArray();

            float iconSize = r.height + 4;
            r.width -= iconSize;

            GUIContent label = new GUIContent(property.displayName, property.tooltip);
            if (warnIfNull && property.objectReferenceValue == null)
                label.image = EditorGUIUtility.IconContent("console.warnicon.sml").image;
            EditorGUI.PropertyField(r, property, label);

            r.x += r.width; r.width = iconSize; r.height = iconSize;
            if (GUI.Button(r, EditorGUIUtility.IconContent("_Popup"), GUI.skin.label))
            {
                GenericMenu menu = new GenericMenu();
                if (property.objectReferenceValue != null)
                {
                    menu.AddItem(new GUIContent("Edit"), false, () 
                        => Selection.activeObject = property.objectReferenceValue);
                    menu.AddItem(new GUIContent("Clone"), false, () => 
                        {
                            ScriptableObject asset = property.objectReferenceValue as ScriptableObject;
                            string fromPath = AssetDatabase.GetAssetPath(asset);
                            string toPath = AssetDatabase.GenerateUniqueAssetPath(fromPath);
                            if (AssetDatabase.CopyAsset(fromPath, toPath))
                            {
                                asset = AssetDatabase.LoadAssetAtPath(
                                    toPath, EmbeddedAssetType(property)) as ScriptableObject;
                                AssetDatabase.SaveAssets();
                                AssetDatabase.Refresh();
                                property.objectReferenceValue = asset;
                                property.serializedObject.ApplyModifiedProperties();
                            }
                        });
                    menu.AddItem(new GUIContent("Locate"), false, () 
                        => EditorGUIUtility.PingObject(property.objectReferenceValue));
                }

                foreach (var t in mAssetTypes)
                {
                    menu.AddItem(new GUIContent("New " + InspectorUtility.NicifyClassName(t.Name)), false, () => 
                        { 
                            string title = "Create New " + t.Name + " asset";
                            ScriptableObject asset = CreateAsset(t, defaultName, title);
                            AssetDatabase.SaveAssets();
                            AssetDatabase.Refresh();
                            property.objectReferenceValue = asset;
                            property.serializedObject.ApplyModifiedProperties();
                        });
                }
                menu.ShowAsContext();
            }
        }

        ScriptableObject CreateAsset(Type assetType, string defaultName, string dialogTitle)
        {
            ScriptableObject asset = null;
            string newAssetPath = EditorUtility.SaveFilePanelInProject(
                    dialogTitle, defaultName, "asset", string.Empty);
            if (!string.IsNullOrEmpty(newAssetPath))
            {
                asset = ScriptableObjectUtility.CreateAt(assetType, newAssetPath);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
            return asset;
        }
    }
}
