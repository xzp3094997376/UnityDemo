using UnityEngine;
using UnityEditor;
using System.Collections;
using System;

public class ExampleClass : EditorWindow
{
    // Add Example1 into a new menu list
    [MenuItem("Example/Example1", false, 100)]    //快捷键显示
    public static void Example1()
    {
        //print("Example/Example1");
        EditorUtility.ClearProgressBar();
    }

    // Add Example2 into the same menu list
    [MenuItem("Example/Example2", false, 100)]
    public static void Example2()
    {
        //print("Example/Example2");
        EditorWindow window= EditorWindow.GetWindow<ExampleClass>();
        window.Show();

        ProcessExecute();
    }

    static bool isShow = false;
    static float process = 0;
    static void ProcessExecute()
    {
        //EditorUtility.DisplayProgressBar("进度条", "资源正在加载", 1f);
        isShow = true;
        startVal = Time.realtimeSinceStartup;
    }

    /// <summary>
    /// 隐藏进度条
    /// </summary>
    static void HideProces()
    {
       // EditorUtility.DisplayCancelableProgressBar("可取消次奥")
    }

    static float startVal = 0;
    void OnInspectorUpdate() //更新  
    {
        Debug.Log("df");
        if (isShow)
        {
            process = (Time.realtimeSinceStartup - startVal) / 5;
            EditorUtility.DisplayProgressBar("进度条", "资源正在加载",process);
            if (process>=1)
            {
                EditorUtility.ClearProgressBar();
                isShow = false;
            }
        }
        //Repaint();//重新绘制 
    }



    #region 验证函数
    [MenuItem("Example/caymanwindow", true,111)]   //用于判断按钮什么时候显示  //条件显示
    static bool ValidateSelection()
    {
        return Selection.activeGameObject != null;
    }
    [MenuItem("Example/caymanwindow", false,112)]   //点击按钮要做的事
    static void Show()
    {
        Debug.Log("Show:" + Selection.activeGameObject.name);
        EditorUtility.OpenFilePanel("选择物体", /*AppDomain.CurrentDomain.BaseDirectory*/ Environment.CurrentDirectory , "cs");//assets目录
        EditorUtility.RevealInFinder(Environment.CurrentDirectory);      //工程根目录浏览
    }
    #endregion

    //层级面板显示
    [MenuItem("GameObject/UI/Custom Game Object", false, 10)]  //用途：右键菜单
    static void CreateCustomGameObject(MenuCommand menuCommand)
    {
        // Create a custom game object
        GameObject go = new GameObject("Custom Game Object");
        // Ensure it gets reparented if this was a context click (otherwise does nothing)
        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
        GameObjectUtility.EnsureUniqueNameForSibling(go);
        // Register the creation in the undo system
        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
        Selection.activeObject = go;
    }


}