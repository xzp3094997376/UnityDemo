using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using LitJson;
using UnityEngine;

[ExecuteInEditMode]
public class JsonTest : MonoBehaviour {
    [SerializeField]
    GameObject go;
    // Start is called before the first frame update
    void Start () {
        go = new GameObject ("g1");
    }

    // Update is called once per frame
    void Update () {
        if (isTest) {
            if (go == null) {
                go = new GameObject ("g1");
            }
            ClassObjectToJson ();
            //ArrToJson();
            JsonToObject ();

        }
    }

    string jsonStr;
    /// <summary>
    /// 实例转换为Json
    /// </summary>
    public bool isTest = false;
    void ClassObjectToJson () {  //类类型和json数据类型的映射
        Pet pet = new Pet ();
        pet.name = "A";
        pet.age = 10;
        pet.isCute = false;
        pet.petColor = new PetColor { r = 1, g = 1, b = 1 };
        pet.intArr = new int[2] { 10, 20 };
        pet.goDic = new Dictionary<string, int> { { "g", 1 } }; //json 支持的数据类型：数字，字符串，逻辑值
        pet.goDic.Add ("123", 10);
        pet.go = null;
        jsonStr = JsonMapper.ToJson (pet);

#if  UNITY_EDITOR
        string dirPath = Application.dataPath + "/LitJson/JsonText";
        if (!Directory.Exists (dirPath)) {
            DirectoryInfo dInfo = Directory.CreateDirectory (dirPath);
            Debug.Log (dInfo.FullName);
        }
        string path = dirPath + "/jsonTest.json";
        using (FileStream fs = new FileStream (path, FileMode.OpenOrCreate)) {
            byte[] byteArr = Encoding.UTF8.GetBytes (jsonStr);
            fs.Write (byteArr, 0, byteArr.Length);
            fs.Flush ();
            fs.Close ();
        }
        UnityEditor.AssetDatabase.Refresh ();
#endif

    }

    /// <summary>
    ///常用类型转换为Json
    /// </summary>
    public void ArrToJson () {
        var array = new string[][] {
            new string[] { "bar", "foo" },
            new string[] { "baz" }
        };

        string str = JsonMapper.ToJson (array);
        Debug.Log (str);
    }
    /// <summary>
    /// 
    /// </summary>
    void JsonToObject () {
        string path = Application.dataPath + "/LitJson/JsonText/jsonTest.json";
        string str = File.ReadAllText (path);
        Pet _pet = JsonMapper.ToObject<Pet> (str);
        Debug.Log (_pet.petColor.r);

    }
}

[System.Serializable]
public class Pet {
    public string name; //共有字段赋值
    public int age;
    string color;
    public bool isCute;
    public PetColor petColor;
    public int[] intArr;
    public Dictionary<string, int> goDic;
    [SerializeField]
    public GameObject go;
    public void Bark () { /***/ }
}

public class PetColor {
    public int r;
    public int g;
    public int b;
}