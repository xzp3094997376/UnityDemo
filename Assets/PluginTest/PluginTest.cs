using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

[ExecuteInEditMode]
public class PluginTest : MonoBehaviour
{
    [DllImport("Dll1")]
    public static extern int MyAddFunc(int x, int y);
    // Start is called before the first frame update
    void Start()
    {
        int ret = MyAddFunc(200, 10);
        Debug.LogFormat("  ret = {0}", ret);
    }

    public bool isTest;
    // Update is called once per frame
    void Update()
    {
        if (isTest)
        {
            isTest = false;
            Start();
        }
    }
}
