using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LookAtPoint))]
[CanEditMultipleObjects]
public class LookAtPointEditor : Editor
{
    SerializedProperty lookAtPoint;

    void OnEnable()
    {
        lookAtPoint = serializedObject.FindProperty("lookAtPoint");
    }

    bool isClick = false;
    /// <summary>
    /// 快捷键设置
    /// </summary>
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(lookAtPoint);
        serializedObject.ApplyModifiedProperties();
        if (lookAtPoint.vector3Value.y > (target as LookAtPoint).transform.position.y)
        {
            EditorGUILayout.LabelField("(Above this object)");
        }
        if (lookAtPoint.vector3Value.y < (target as LookAtPoint).transform.position.y)
        {
            EditorGUILayout.LabelField("(Below this object)");
        }
       
        isClick = EditorGUILayout.Toggle(new GUIContent("toggle"), isClick);
        if (isClick)
        {
            EditorGUILayout.LabelField("( clicked)");
        }
        else
        {
            EditorGUILayout.LabelField("(no clicked)");
        }
    }

    /// <summary>
    /// 获得焦点时调用
    /// </summary>
    public void OnSceneGUI()
    {
        var t = (target as LookAtPoint);

        EditorGUI.BeginChangeCheck();
        Vector3 pos = Handles.PositionHandle(t.lookAtPoint, Quaternion.identity);
        Handles.Label(t.transform.position + Vector3.up * 3, t.name + " : " + t.transform.position.ToString());

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(target, "Move point");
            t.lookAtPoint = pos;
            t.Update();
        }
        Handles.Label(t.transform.position + Vector3.up * 4, "label");

        if (Event.current.type == EventType.Repaint)
        {
            Handles.ArrowHandleCap(
              0,
              t.transform.position + new Vector3(3f, 0f, 0f),
              t.transform.rotation * Quaternion.LookRotation(Vector3.right),
              1,
              EventType.Repaint
          );
        }            
    }
}
