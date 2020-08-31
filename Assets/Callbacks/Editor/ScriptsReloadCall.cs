using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Callbacks;
using UnityEditor;

public class ScriptsReloadCall
{
    /// <summary>
    /// 脚本加载完毕之后调用
    /// </summary>
    [DidReloadScripts]
    public static void ReloadCall()
    {
        Debug.Log("ReloadCall");

        Light light= GameObject.FindObjectOfType<Light>();
        light.intensity = 10;

        DialogDisplay();
    }

    static void DialogDisplay()
    {
        Debug.Log("dialog dispPlay");
        EditorUtility.DisplayDialog("加载", "加载中", "加载完毕");
    }



    /// <summary>
    /// 双击打开资源时调用
    /// </summary>
    /// <param name="instanceID"></param>
    /// <param name="line"></param>
    /// <returns></returns>
    [OnOpenAssetAttribute(1)]
    public static bool step1(int instanceID, int line)
    {
        string name = EditorUtility.InstanceIDToObject(instanceID).name;
        Debug.Log("Open Asset step: 1 (" + name + ")");
        return false; // we did not handle the open
    }

    // step2 has an attribute with index 2, so will be called after step1
    [OnOpenAssetAttribute(2)]
    public static bool step2(int instanceID, int line)
    {
        Debug.Log("Open Asset step: 2 (" + instanceID + ")");
        return false; // we did not handle the open
    }

  
}
