using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class MyWindow1 : EditorWindow
{
    string myString = "Hello World";
    bool groupEnabled;
    bool myBool=true,Mybool1 = true;
    float myFloat = 1.23f;

    // 将名为"My Window"的菜单项添加到 Window 菜单
    [MenuItem("Window/GUIWindow")]
    public static void ShowWindow()
    {   
        //显示现有窗口实例。如果没有，请创建一个。
        EditorWindow.GetWindow(typeof(MyWindow1));
    }

    GameObject Obj ;
    void OnGUI()
    {
        GUILayout.Label("Base Settings", EditorStyles.boldLabel);
        myString = EditorGUILayout.TextField("Text Field", myString);

        EditorGUILayout.BeginHorizontal();
         Obj= EditorGUILayout.ObjectField("字段", Obj, typeof(GameObject), false) as GameObject;
        //EditorGUILayout.ObjectField("字段", Obj, typeof(GameObject), true) /*as GameObject*/;
        EditorGUILayout.EndHorizontal();


        groupEnabled = EditorGUILayout.BeginToggleGroup("Optional Settings", groupEnabled);
        myBool = EditorGUILayout.Toggle("Toggle", myBool);
        Mybool1 = EditorGUILayout.Toggle("Toggle1", Mybool1);
        myFloat = EditorGUILayout.Slider("Slider", myFloat, -3, 3);
        EditorGUILayout.EndToggleGroup();
    }
}
