using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectionGetDepth : MonoBehaviour
{
    public Shader replaceShader;

    [Range(0f, 2f)]
    public float depthScale = 0.33333f;
    public float depthCameraNearMove = 0.0f;

    protected Camera depthCamera;


    void Start()
    {
        depthCamera = GetComponent<Camera>();

        float cameraNear = depthCamera.nearClipPlane;
        cameraNear += depthCameraNearMove;
        float cameraFar = depthCamera.farClipPlane;
        cameraFar = cameraFar * depthScale;


        
        if (replaceShader == null)
        {
            replaceShader = Shader.Find("Depth/DepthShader");
            //Debug.LogError("find depth shader is null");
        }

        //StartCoroutine(ChangeShader());
        depthCamera.SetReplacementShader(replaceShader, "");


        ReplaceDepthScale(depthScale);
        ReplaceDepthCameraNearMove(depthCameraNearMove);
    }


    //private void Update()
    //{
    //    ReplaceDepthScale(depthScale);
    //    ReplaceDepthCameraNearMove(depthCameraNearMove);
    //}



    public void ReplaceDepthScale(float depthScale)
    {
        float cameraFar = depthCamera.farClipPlane;
        cameraFar = cameraFar * depthScale;
        //Debug.Log("设置cameraFar为" + cameraFar);
        Shader.SetGlobalFloat("_DepthShaderCameraFar", cameraFar);
    }

    public void ReplaceDepthCameraNearMove(float nearMove)
    {
        float num = this.depthCamera.nearClipPlane;
        num += nearMove;
        //Debug.Log("设置num为" + num);
        Shader.SetGlobalFloat("_DepthShaderCameraNear", num);
    }






}
