using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

[ExecuteInEditMode]
public class TestProcess : MonoBehaviour
{
    // Start is called before the first frame update
    void OnEnable()
    {
        Process[] processes = Process.GetProcesses();
        foreach (Process p in processes)
        {
            UnityEngine.Debug.Log(p.ProcessName+"  "+p.BasePriority);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private static void myProcess_HasExited(object sender, System.EventArgs e)
    {
        Console.WriteLine("Process has exited.");
    }
}
