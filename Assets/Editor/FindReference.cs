using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

public class FindReference 
{
    static string dirpath = "ModelTest";
    static List<string> shieldDir = new List<string>() { "jxjc", "fdjgzywx", "qcdpgzywx", "xnyqcqddjjs", "model" };
    [MenuItem("Assets/AllReference(反向关联查找)")]
    public static void AllReferenceFindResource()
    {
        EditorSettings.serializationMode = SerializationMode.ForceText;

        string fullDir = Application.dataPath + "/" + dirpath;
        var notwithhoutExtensions = new List<string>() { ".meta" };
        // 需要反向查找的资源
        string[] files = Directory.GetFiles(fullDir, "*.*", SearchOption.AllDirectories)
                .Where(s => !notwithhoutExtensions.Contains(Path.GetExtension(s).ToLower())).ToArray();

        // 目标资源
        var targetExtensions = new List<string>() { ".prefab", ".unity", ".fbx", ".mat", ".asset" };
        string[] targetFiles = Directory.GetFiles(Application.dataPath + "/ModelTest", "*.*", SearchOption.AllDirectories)
                .Where(s => targetExtensions.Contains(Path.GetExtension(s).ToLower())).ToArray();


        int startIndex = 0;
        //EditorApplication.update = delegate()
        {
            for (int i = 0; i < files.Length; i++)
            {
                string assetpath = files[i].Replace("\\", "/").Replace(Application.dataPath, "Assets");

                string guid = AssetDatabase.AssetPathToGUID(assetpath);
                Debug.Log(guid);
                for (int j = 0; j < targetFiles.Length; j++)
                {
                    string _target = targetFiles[j];

                    bool isCancel = EditorUtility.DisplayCancelableProgressBar("资源匹配中", _target, (float)startIndex / (float)targetFiles.Length);
                    string txt = File.ReadAllText(_target);
                    Debug.Log(txt);
                    if (Regex.IsMatch(txt/*File.ReadAllText(_target)*/, guid))
                    {
                        Debug.LogError(guid);
                        string taregtpath = _target.Replace("\\", "/").Replace(Application.dataPath + "/", "");
                        string[] shield = taregtpath.Split('/');
                        if (shield.Length > 0)
                        {
                            if (!shieldDir.Contains(shield[0]))
                            {
                                Debug.Log(assetpath.Replace("Assets/", "") + "-->>  " + _target.Replace("\\", "/").Replace(Application.dataPath + "/", "") + ": ");
                            }
                        }
                        else
                            Debug.Log(assetpath.Replace("Assets/", "") + "-->>  " + _target.Replace("\\", "/").Replace(Application.dataPath + "/", "") + ": ");
                    }
                    if (isCancel)
                    {
                        //EditorUtility.ClearProgressBar();
                        //EditorApplication.update = null;
                        Debug.Log("匹配结束  匹配数量： " + startIndex + "  总数量： " + targetFiles.Length);
                        startIndex = 0;
                        break;
                    }
                }

            }

            EditorUtility.ClearProgressBar();
            //EditorApplication.update = null;
        };


    }
}
