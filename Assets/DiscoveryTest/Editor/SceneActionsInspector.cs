//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEditor;

//using UnityEditorInternal;

//[CustomEditor(typeof(SceneActions))]
//public class SceneActionsInspector : Editor
//{
//    private SceneActions data;
//    private ReorderableList guiList;

//    private void OnEnable()
//    {
//        if ( data == null )
//        {
//            data = (SceneActions)target;
//        }

//        if ( guiList == null )
//        {
//            guiList = new ReorderableList(serializedObject, serializedObject.FindProperty("sceneActions"), true, true, true, true);
//            guiList.drawElementCallback = drawElement;
//        }
//    }

//    public override void OnInspectorGUI()
//    {
//        serializedObject.Update();
//        guiList.DoLayoutList();
//        serializedObject.ApplyModifiedProperties();
//    }

//    private void drawHeader(Rect rect)
//    {
//        EditorGUI.LabelField(rect, "Scene Actions");
//    }

//    private void drawElement(Rect _rect, int _index, bool _isActive, bool _isFocused)
//    {
//        var element = guiList.serializedProperty.GetArrayElementAtIndex(_index);
//        _rect.y += 2;

//        EditorGUI.PropertyField(new Rect(_rect.x, _rect.y, 60, EditorGUIUtility.singleLineHeight), element.FindPropertyRelative("text"), GUIContent.none);
//        EditorGUI.PropertyField(new Rect(_rect.x + 60, _rect.y, _rect.width - 60, EditorGUIUtility.singleLineHeight), element.FindPropertyRelative("action"), GUIContent.none);
//    }
//}
