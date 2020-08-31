using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MyWindow : EditorWindow
{
    [MenuItem("Window/My Window #&g")]//

    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(MyWindow));
    }
    private void OnEnable()
    {
        Texture2D txture= new Texture2D(1000,1000);
        txture = icon;
    }
    public Texture2D icon;
    bool isclick = false;
    string content = "no clicked";
    void OnGUI()  //窗口获得焦点时被激活 每一帧绘制
    {
       
        // 此处为实际窗口代码
        GUILayout.BeginArea(new Rect(Vector2.zero, Vector2.one * 200),icon);
        GUILayout.Label(new GUIContent("window",icon,"这是一个窗口"), new GUIStyle());
        GUILayout.BeginScrollView(Vector2.zero);
     
        if (GUILayout.Button("button"))
        {
            content = "clicked";           
        }

        if (GUILayout.Button("Asset_Btn"))
        {
            AssetDataBaseTest();
        }
        GUI.Label(new Rect(150, 150, 100, 100), new GUIContent(content, "button clicked"));
        GUILayout.EndScrollView();
        GUILayout.EndArea();       

        
    }

    /// <summary>
    /// 项目资源加载到内存
    /// </summary>
    void AssetDataBaseTest()
    {
        Debug.Log("button clicked");
        //icon= AssetDatabase.LoadAssetAtPath(@"D:/UnityProject/Test/Assets/Demigiant/DemiLib/Core/Editor/Imgs/greenSquare.png", typeof(Texture2D)) as Texture2D;
        icon = AssetDatabase.LoadAssetAtPath("Assets/Demigiant/DemiLib/Core/Editor/Imgs/greenSquare.png", typeof(Texture2D)) as Texture2D;
        Debug.Log(icon.name);

        AssetDatabase.ExportPackage("Assets/Demigiant/DemiLib/Core/Editor/Imgs/greenSquare.png", "Assets/Demigiant/DemiLib/Core/Editor/Imgs/1.unitypackage");
        AssetDatabase.Refresh();
    }

    /// <summary>
    /// 找到对应的资源
    /// </summary>
    void SearchAssets()
    {
        //Editor.Destroy()
    }
}
