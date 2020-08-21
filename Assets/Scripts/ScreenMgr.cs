using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class ScreenMgr : MonoBehaviour
{
    private void Awake()
    {       
    }
    // Start is called before the first frame update
    void Start()        
    {
        CheckScreenNum();
    }

    // Update is called once per frame
    int CheckScreenNum()
    {
        int num = Display.displays.Length;
        for (int i = 0; i < Display.displays.Length; i++)
        {

            Display.displays[i].Activate();//激活连接主机的所有屏幕，并且激活之后不能再失活

            //Screen.SetResolution(Display.displays[i].renderingWidth, Display.displays[i].renderingHeight, true);
            //Display.displays[i].SetRenderingResolution(/*Display.displays[i].renderingWidth*/250, /*Display.displays[i].renderingHeight*/250);

            //Debug.Log(Display.displays[i].renderingWidth + ",  " + Display.displays[i].renderingHeight);
            //Debug.Log(Display.displays[i].systemWidth + ",  " + Display.displays[i].systemHeight);//编辑器下默认是game窗口分辨率
        }

       // Camera.main.SetTargetBuffers(Display.main.colorBuffer, Display.main.depthBuffer);
        return num;
    }

    Camera cam2d;
    Camera camModel;
    public Transform rightBottom;
    public Transform ui_2d;
    Transform content;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("开启扩展屏");
            //2d相机激活扩展到2屏幕
            cam2d = GameObject.Find("Camera_ui").GetComponent<Camera>();
            cam2d.targetDisplay = 1;

            //模型相机不渲染扩展画布
            camModel= GameObject.Find("Camera_model").GetComponent<Camera>();
            camModel.cullingMask = ~(1 << LayerMask.NameToLayer("2d"));

        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("关闭扩展屏");
            cam2d.targetDisplay = 0;
            
            //2d画布内容切换到主屏
            content=GameObject.Find("2d_canvas_1/Image").transform;
            content.localScale = Vector3.one;            

        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log("点击模型视图按钮");            
            //画布缩小
            content.DOScale(Vector3.one * 0.1f, 0.5f);
            ui_2d.DOMove(rightBottom.position, 0.5f);
            //模型相机渲染
            camModel.cullingMask |= (1 << LayerMask.NameToLayer("2d"));

        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            Debug.Log("点击缩小的画布切换到2d状态");
            content.DOScale(Vector3.one , 0.5f);
             ui_2d.DOLocalMove(Vector3.zero, 0.5f);
        }
    }

    void UpdateEx()
    {
        //if (Display.displays.Length > 1 && !extCam.enabled)
        //{
        //    Display.displays[1].SetRenderingResolution(256, 256);
        //    extCam.SetTargetBuffers(Display.displays[1].colorBuffer, Display.displays[1].depthBuffer);
        //}
        //extCam.enabled = Display.displays.Length > 1;
    }
}
