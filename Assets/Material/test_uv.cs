using UnityEngine;
using System.Collections;

public class test_uv : MonoBehaviour
{
    public float xspeed = 0;
    public float yspeed = 0;
    private Vector2 v2;
    private Renderer renderer;

    [Header("锁定纹理缩放")]
    public bool isEnableTextureTilingLock = true;
    //动画总帧数
    public int aniFPS;
    //模型缩放区间
    public Vector3 modelScaleStart;//起始
    public Vector3 modelScaleEnd;//结束
    [HideInInspector]
    public Vector3 modelScaleSection;//区间
    [HideInInspector]
    public Vector3 modelScaleFPS;//模型缩放单帧值
    //材质Tiling区间
    public Vector2 materialsTilingStart = new Vector2(1, 1);//起始
    public Vector2 materialsTilingEnd = new Vector2(1, 1);//结束
    [HideInInspector]
    public Vector2 materialsTilingSection;//区间
    [HideInInspector]
    public Vector2 materialsTilingFPS;//材质Tiling单帧值


    void Start()
    {
        v2 = Vector2.zero;
        //AssetBundleManager.GetBundle("dataconfig", getdata);
        renderer = GetComponent<Renderer>();

        isEnableTextureTilingLock = !(aniFPS <= 0);//为默认值关闭锁定
        UpdateValue();
    }

    void FixedUpdate()
    {
        v2.x += Time.fixedDeltaTime * xspeed;
        v2.y += Time.fixedDeltaTime * yspeed;
        if (renderer)
        {
            renderer.materials[0].mainTextureOffset = v2;
            if (isEnableTextureTilingLock) TextureTilingLock();
        }
    }

    private void UpdateValue()
    {
        //模型缩放单帧值
        modelScaleSection = new Vector3(
            modelScaleStart.x - modelScaleEnd.x,
            modelScaleStart.y - modelScaleEnd.y,
            modelScaleStart.z - modelScaleEnd.z);
        modelScaleFPS = new Vector3(
            modelScaleSection.x / aniFPS,
            modelScaleSection.y / aniFPS,
            modelScaleSection.z / aniFPS);
        //材质Tiling单帧值
        materialsTilingSection = new Vector3(
            materialsTilingStart.x - materialsTilingEnd.x,
            materialsTilingStart.y - materialsTilingEnd.y);
        materialsTilingFPS = new Vector3(
            materialsTilingSection.x / aniFPS,
            materialsTilingSection.y / aniFPS);
    }

    //计算锁定纹理Tiling值
    private void TextureTilingLock()
    {
        UpdateValue();

        //计算当前帧物体缩放的变换值
        Vector3 objScale = transform.localScale - modelScaleStart;
        //计算各轴变换比
        int conversionX = (int)(objScale.x / modelScaleFPS.x);
        int conversionY = (int)(objScale.y / modelScaleFPS.y);
        int conversionZ = (int)(objScale.z / modelScaleFPS.z);

        //等比代换到Tiling
        Vector2 t = new Vector2(materialsTilingFPS.x * conversionX, materialsTilingFPS.y * conversionY);

        renderer.materials[0].mainTextureScale = materialsTilingStart + t;

        //transform.localScale=modelScaleStart+

        //for (int i = 0; i < renderer.materials.Length; i++)
        //{

        //}
    }
}
