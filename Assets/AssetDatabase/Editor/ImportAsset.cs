using UnityEngine;
using UnityEditor;

public class ImportAsset
{
    [MenuItem("AssetDatabase/ImportExample")]
    static void ImportExample()
    {
        AssetDatabase.ImportAsset("Assets/circle.png", ImportAssetOptions.Default);
    }

    [MenuItem("AssetDatabase/LoadAssetExample")]
    static void LoadAsset()
    {
        Texture2D t = AssetDatabase.LoadAssetAtPath("Assets/circle.png", typeof(Texture2D)) as Texture2D;//导入设置配种
        string path= AssetDatabase.GetAssetPath(t);
        TextureImporter import= TextureImporter.GetAtPath(path) as TextureImporter;
        import.textureType = TextureImporterType.Sprite;
        import.spritePackingTag = "importAsset";

        TextureImporterPlatformSettings pset = import.GetDefaultPlatformTextureSettings();
        pset.overridden = true;
        pset.maxTextureSize = 2048;
        pset.textureCompression = TextureImporterCompression.CompressedHQ;
        pset.format = TextureImporterFormat.RGB16;
        import.SetPlatformTextureSettings(pset);

        TextureImporterPlatformSettings _pset = import.GetPlatformTextureSettings("PC");
        _pset.maxTextureSize = 4096;
        _pset.textureCompression = TextureImporterCompression.CompressedHQ;        

        import.SetPlatformTextureSettings(_pset);        

        AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);//重新导入
        
    }

    [MenuItem("AssetDatabase/CreateAsset")]
    static void CreateAsset()
    {
        //GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //go.hideFlags = HideFlags.NotEditable;
        //Debug.Log("资源创建");        

        GameObject go= Selection.activeGameObject;
        Component[] comps= go.GetComponents<Component>();
        SerializedObject sObj= new SerializedObject(go);
        for (int i = 0; i < comps.Length; i++)
        {
            Debug.Log(comps[i]);
            if (comps[i]==null)
            {
                GameObject.DestroyImmediate(comps[i]);
            }
        }
        sObj.ApplyModifiedProperties();
            
    }

    [MenuItem("MyTools/Delete Missing Scripts")]
    static void CleanupMissingScript()
    {
        GameObject[] pAllObjects = (GameObject[])Resources.FindObjectsOfTypeAll(typeof(GameObject));

        int r;
        int j;
        for (int i = 0; i < pAllObjects.Length; i++)
        {
            if (pAllObjects[i].hideFlags == HideFlags.None)//HideFlags.None 获取Hierarchy面板所有Object
            {
                var components = pAllObjects[i].GetComponents<Component>();
                var serializedObject = new SerializedObject(pAllObjects[i]);
                var prop = serializedObject.FindProperty("m_Component");
                r = 0;

                for (j = 0; j < components.Length; j++)
                {
                    if (components[j] == null)
                    {
                        prop.DeleteArrayElementAtIndex(j - r);
                        r++;
                    }
                }

                serializedObject.ApplyModifiedProperties();
            }
        }
    }
}