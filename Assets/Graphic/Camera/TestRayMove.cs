using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRayMove : MonoBehaviour
{
    public bool zooming;
    public float zoomSpeed;
    public Camera camera;
    void Update()
    {
        if (zooming)
        {
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(ray.origin, ray.direction*1000, Color.red, 0f,false);
            float zoomDistance = zoomSpeed * Input.GetAxis("Vertical") * Time.deltaTime;    
            camera.transform.Translate(ray.direction * zoomDistance, Space.World);
        }
    }
}
