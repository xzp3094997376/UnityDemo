using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.Callbacks;
using UnityEngine;

public class OnBuildPost: IPreprocessBuildWithReport
{
    public int callbackOrder =>0;

    [PostProcessBuildAttribute(1)]
    public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
    {
        Debug.Log(pathToBuiltProject);
        Debug.Log(target);
        string buildStr=PlayerSettings.GetScriptingDefineSymbolsForGroup((BuildTargetGroup)EditorUserBuildSettings.activeBuildTarget);
        Debug.Log(buildStr);
        throw new System.OperationCanceledException("Build was canceled by the user.");

    }

    public void OnPreprocessBuild(BuildReport report)
    {
        foreach (var item in report.files)
        {
            Debug.Log(string.Format("path:{0},  role:{1}, size: {2}", item.path, item.role, item.size));
        }
        PlayerSettings.companyName = "GTRA";
        throw new UnityEditor.Build.BuildFailedException("user canceled");


    }
}
