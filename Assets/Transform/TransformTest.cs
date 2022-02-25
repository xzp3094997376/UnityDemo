using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformTest : MonoBehaviour
{
    public Transform tr;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(tr.forward);
        Vector3 dir = tr.TransformDirection(tr.forward);
        Debug.Log(dir);
        Debug.DrawRay(tr.position, dir * 10, Color.red, 100);
    }

    // Update is called once per frame
    void Update()
    {
      ;
    }
}
