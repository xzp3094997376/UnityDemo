using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LogConditonTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.Logger(this.name);
#if UNITY_EDITOR         
#endif
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
