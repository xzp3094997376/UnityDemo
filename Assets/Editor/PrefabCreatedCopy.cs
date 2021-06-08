using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
public class PrefabCreatedCopy  {
    
    [MenuItem("Assets/创建预制体到书中")]
    static void Created()
    {
        GameObject go = Selection.activeObject as GameObject;

        string parName= GUIUtility.systemCopyBuffer;
        Debug.Log("剪切板内容: " + parName);

        GameObject par= new GameObject(parName);
        par.transform.localPosition = Vector3.zero;

        go = PrefabUtility.InstantiateAttachedAsset(go) as GameObject;
        go.name= go.name.Split('(')[0];
        go.transform.SetParent(par.transform);
        go.transform.localPosition = Vector3.zero;
        //go.transform.localScale = Vector3.one;

        string savePath =Application.dataPath+"/"+ parName;
        int _index = savePath.LastIndexOf("_");
        savePath= savePath.Substring(0, _index);
        savePath= savePath.Replace("_", "/");
        Debug.Log(savePath);
        if (!Directory.Exists(savePath))
        {
            Directory.CreateDirectory(savePath);
        }
        Debug.Log("dataPath: "+Application.dataPath);
        savePath =savePath.Remove(0,savePath.LastIndexOf("Assets"));

        Debug.Log("savePath:  "+savePath);
        string prefabSavePath = savePath +"/"+parName+".prefab";
        Debug.Log("预制体保存路径 "+prefabSavePath);


        GameObject prefab = PrefabUtility.CreatePrefab(prefabSavePath, par);
        PrefabUtility.ConnectGameObjectToPrefab(par, prefab);
        //ModelControl control= prefab.GetComponent<ModelControl>();
        //if (control==null)
        //{
        //    prefab.AddComponent<ModelControl>();
        //}

        EditorGUIUtility.PingObject(par);//设置焦点
        Selection.activeGameObject = par;

        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();

    }
}
