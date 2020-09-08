using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

public class BuildPlayerTest
{
    [MenuItem("MyTools/Windows Build With Postprocess")]
    public static void BuildGame()
    {
        // 获取文件名。
        string path = EditorUtility.SaveFolderPanel("Choose Location of Built Game", "", "");
        string[] levels = new string[] { "Assets/BuildPlayer/BuildPlayer.unity"};

        // 构建播放器。
        BuildReport report= BuildPipeline.BuildPlayer(levels, path + "/BuiltGame.exe", BuildTarget.StandaloneWindows, BuildOptions.None);
        UnityEngine.Debug.Log(report);

        // 将文件从项目文件夹复制到构建文件夹，与构建的游戏放在一起。
        FileUtil.CopyFileOrDirectory(@"Assets/Readme.txt", path + "/Readme.txt");

        // 运行游戏（System.Diagnostics 中的 Process 类）。
        Process proc = new Process();
        proc.StartInfo.FileName = path + "/BuiltGame.exe";
        proc.Start();
    }
}
