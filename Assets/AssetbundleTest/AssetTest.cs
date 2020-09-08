using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;

[ExecuteInEditMode]
public class AssetTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (unloadall)
        {
            AssetBundle.UnloadAllAssetBundles(false);
            Debug.Log("卸载所有的Assets");
        }
        if (isLoadAsset)
        {
            isLoadAsset = false;

            //StopAllCoroutines();
            StartCoroutine(InstantiateObject());

            return;


            //AssetBundle[] assetArr = AssetBundle.GetAllLoadedAssetBundles().ToArray();
            //Debug.Log(assetArr.Length);
            //foreach (var item in assetArr)
            //{
            //    Debug.Log(item.name);
            //}
            //if (assetArr.Length!=0)
            //{
            //    AssetBundle ab = AssetBundle.GetAllLoadedAssetBundles()?.First((a) => { return a.name == "cube.assetbundle"; });
            //    if (ab != null)
            //    {
            //        Debug.Log("hasLoad");
            //        Instantiate(ab.LoadAsset<GameObject>("Cube"));
            //        //return;
            //    }
            //}
            //else
            //{
            //    StartCoroutine(InstantiateObject());
            //}
            //StartCoroutine(InstantiateObject());//已经被加载过，不能被再次加载
        }
       
    }

    public bool isLoadAsset;
    public bool unloadall;
    IEnumerator InstantiateObject() //本地hu
    {
        yield return StartCoroutine(LoadAssetBundleManifest());

        string uri = "file:///" + Application.dataPath + "/AssetbundleTest/AssetBundles/" + "cube.assetbundle";
        uri = Application.streamingAssetsPath + "/AssetBundles/cube.assetbundle";
        AssetBundle ar = AssetBundle.LoadFromFile(uri);
        GameObject obj= ar.LoadAsset<GameObject>("cube"); 
        GameObject go= Instantiate(obj) as GameObject;
        go.name = "cube";
        Debug.Log(go.name);
        ;
        //UnityEngine.Networking.UnityWebRequest request = UnityEngine.Networking.UnityWebRequestAssetBundle.GetAssetBundle(uri, 0);
        //yield return request.SendWebRequest();
        //AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(request);
        //AssetBundleRequest ar = bundle.LoadAssetAsync<GameObject>(bundle.name.Split('.' )[0]);
        //yield return ar;


        //GameObject go=Instantiate(ar.asset) as GameObject;
        ////GameObject sprite = bundle.LoadAsset<GameObject>("Sprite");     
        //go.name = "assetload";
#if UNITY_EDITOR
        AssetDatabase.Refresh();
#endif
    }

    /// <summary>
    /// 加载对应的AssetBundleManifest 文件 处理依赖
    /// </summary>
    IEnumerator LoadAssetBundleManifest()
    {
        yield return 1;
        string uri = "file:///" + Application.dataPath + "/AssetbundleTest/AssetBundles/" + "AssetBundles";//
        uri = Application.streamingAssetsPath + "/AssetBundles/AssetBundles";
        AssetBundle bundle = AssetBundle.LoadFromFile(uri);
        //UnityEngine.Networking.UnityWebRequest request = UnityEngine.Networking.UnityWebRequestAssetBundle.GetAssetBundle(uri, 0);
        //yield return request.SendWebRequest();
        //AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(request);
        //yield return bundle;
        //Debug.Log(bundle.name);
        //AssetBundleRequest ar = bundle.LoadAssetAsync<AssetBundleManifest>("AssetBundleManifest");
        yield return 1;
        AssetBundleManifest amf =bundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
        string[] dependencies = amf.GetAllDependencies("cube.assetbundle");
        
        foreach (string item in dependencies)
        {
            Debug.Log(item);
            string path= Path.Combine(uri.Substring(0, uri.LastIndexOf('/')),item);
            path = path.Replace(@"\",@"/");
            Debug.Log(path);
            AssetBundle.LoadFromFile(path);
        }
    }
}

public  class PathTool {
    public static string StreamingAssetsPath
    {
        get
        {
#if UNITY_STANDALONE
            return Application.dataPath + "/StreamingAssets";
#elif UNITY_IOS
            return Application.dataPath + "/Raw";
#elif UNITY_ANDROID
            retun "jar:file://" + Application.dataPath + "!/assets/";
#endif    
        }
    }
}


