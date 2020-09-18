using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[ExecuteInEditMode]
public class ScrollTest : MonoBehaviour
{
    ScrollRect sr;
    // Start is called before the first frame update
    void Start()
    {
        sr= GetComponent<ScrollRect>();
    }

    public bool isTest;
    public float normal;
    // Update is called once per frame
    void Update()
    {
        if (isTest)
        {
            normal= sr.verticalNormalizedPosition;
        }
    }
}
