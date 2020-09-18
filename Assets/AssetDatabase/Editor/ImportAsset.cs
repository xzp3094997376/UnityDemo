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
}