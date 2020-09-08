using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

[ExecuteInEditMode]
public class JsonUtilityTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public bool isToJson;
    // Update is called once per frame
    void Update()
    {
        if (isToJson)
        {
            isToJson = false;
            ToJson();
        }
        if (isJsonToObj)
        {
            isJsonToObj = false;
            JsonToObject();
        }
    }

    public void ToJson()
    {
        MyClass myObject = new MyClass();
        myObject.level = 1;
        myObject.timeElapsed = 47.5f;
        myObject.playerName = "Dr Charles Francis";

        string json = JsonUtility.ToJson(myObject);
        //json=json.Replace(",", ","+Environment.NewLine);
        json = json.Replace(",", ",\n");
        WriteTextToLocal(json);
    }

    void WriteTextToLocal(string jsonStr)
    {
#if UNITY_EDITOR
        string dirPath = Application.dataPath + "/JsonUtility/JsonText";
        if (!Directory.Exists(dirPath))
        {
            DirectoryInfo dInfo = Directory.CreateDirectory(dirPath);
            Debug.Log(dInfo.FullName);
        }
        string path = dirPath + "/jsonTest.json";
        using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
        {
            byte[] byteArr = Encoding.UTF8.GetBytes(jsonStr);
            fs.Write(byteArr, 0, byteArr.Length);
            fs.Flush();
            fs.Close();
        }
        UnityEditor.AssetDatabase.Refresh();
#endif
    }

    public bool isJsonToObj = false;         
    void JsonToObject()
    {
        string _path = Application.dataPath + "/JsonUtility/JsonText/jsonTest.json";
        string jsonStr= File.ReadAllText(_path);
        MyClass mc= JsonUtility.FromJson<MyClass>(jsonStr);
        Debug.Log(mc.level + " " + mc.playerName + " " + mc.timeElapsed);

    }
}


[Serializable]
public class MyClass
{
    public int level;
    public float timeElapsed;
    public string playerName;
}