using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestZoom : MonoBehaviour
{
    public Transform target;
    public Camera camera;

    private float initHeightAtDist;
    private bool dzEnabled;

    // 计算距摄像机一定距离的视锥体高度。
    float FrustumHeightAtDistance(float distance)
    {
        return 2.0f * distance * Mathf.Tan(camera.fieldOfView * 0.5f * Mathf.Deg2Rad);
    }

    // 计算在给定距离处获得给定视锥体高度所需的 FOV。
    float FOVForHeightAndDistance(float height, float distance)
    {
        return 2.0f * Mathf.Atan(height * 0.5f / distance) * Mathf.Rad2Deg;
    }

    //启动推拉变焦效果。
    void StartDZ()
    {
        var distance = Vector3.Distance(transform.position, target.position);
        initHeightAtDist = FrustumHeightAtDistance(distance);
        dzEnabled = true;
    }

    // 关闭推拉变焦。
    void StopDZ()
    {
        dzEnabled = false;
    }

    void Start()
    {
        StartDZ();
    }

    void Update()
    {
        if (dzEnabled)
        {
            //测量新距离并相应重新调整 FOV。
            var currDistance = Vector3.Distance(transform.position, target.position);
            camera.fieldOfView = FOVForHeightAndDistance(initHeightAtDist, currDistance);
        }

        //采用简单控制方式，允许使用向上/向下箭头来移入和移出摄像机。
        transform.Translate(Input.GetAxis("Vertical") * Vector3.forward * Time.deltaTime * 5f);
    }
}

