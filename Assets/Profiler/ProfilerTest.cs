using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProfilerTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        UnityEngine.Profiling.Profiler.BeginSample("MyPieceOfCode");
        Transform canvas= GameObject.Find("Canvas").transform; 
        // Code to measure...
        for (int i = 0; i < 100; i++)
        {
            GameObject go = Instantiate(Resources.Load<GameObject>("Image"));
            go.transform.SetParent(canvas);
        }
        UnityEngine.Profiling.Profiler.EndSample();
    }

    // Update is called once per frame
    void Update()
    {
        Start();
    }
}
