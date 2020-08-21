using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUITest : MonoBehaviour//运行时做测试用
{
    public Texture2D icon;
    public bool toggleBool;
    public string textAreaString;
    void OnGUI()
    {
        // 创建背景框
        GUI.Box(new Rect(10, 10, 100, 90), "Loader Menu");

        // 创建第一个按钮。如果按下此按钮，则会执行 Application.Loadlevel (1)
        if (GUI.Button(new Rect(20, 40, 80, 20), "Level 1"))
        {
            PlayerPrefs.SetString("screenMode", screen.single.ToString("g"));
        }

        string mode = "none";
        // 创建第二个按钮。
        if (GUI.Button(new Rect(20, 70, 80, 20), "Level 2"))
        {         
            mode= PlayerPrefs.GetString("screenMode",mode);
            Debug.LogError(mode);
        }

        GUI.Label(new Rect(50, 200, 100, 50), "This is the screen:  "+mode);
        if (GUI.Button(new Rect(20, 90, 100, 50), icon))
        {
            print("you clicked the icon");
        }

        GUI.Box(new Rect(100, 100, 100, 50), new GUIContent("This is text", icon));


        toggleBool = GUI.Toggle(new Rect(25, 25, 100, 30), toggleBool, "Toggle");


        textAreaString = GUI.TextArea(new Rect(25, 25, 100, 30), textAreaString);


        ToolbarTest();

        SelectionGridTest();

        ScrollViewTest();

        WindowTest();
    }


    /// <summary>
    /// 类似toggleGroup
    /// </summary>
    public int toolbarInt = 0;
    private string[] toolbarStrings = { "Toolbar1", "Toolbar2", "Toolbar3" };
    void ToolbarTest()
    {
        toolbarInt = GUI.Toolbar(new Rect(25, 25, 250, 30), toolbarInt, toolbarStrings);
    }

    private int selectionGridInt = 0;
    private string[] selectionStrings = { "Grid 1", "Grid 2", "Grid 3", "Grid 4" };
    void SelectionGridTest()
    {
        selectionGridInt = GUI.SelectionGrid(new Rect(25, 25, 300, 60), selectionGridInt, selectionStrings, 2);
    }


    public Vector2 scrollViewVector = Vector2.zero;
    private string innerText = "I am inside the ScrollView";
    private void ScrollViewTest()
    {
        // 开始 ScrollView
        scrollViewVector = GUI.BeginScrollView(new Rect(325, 325, 100, 100), scrollViewVector, new Rect(0, 0, 400, 400));

        // 在 ScrollView 中放入一些内容
        innerText = GUI.TextArea(new Rect(0, 0, 400, 400), innerText);

        // 结束 ScrollView
        GUI.EndScrollView();
    }

    private Rect windowRect = new Rect(520, 120, 120, 50);
    void WindowTest()
    {
        windowRect = GUI.Window(0, windowRect, (_id)=> {
            GUI.Label(new Rect(20, 20, 100, 20), "window label");

        }, "My Window");
    }
}
enum screen
{
    single,
    dual,
}