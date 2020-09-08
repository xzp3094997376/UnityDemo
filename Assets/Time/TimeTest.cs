using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeTest : MonoBehaviour
{
    public float time_Scale=1;
    // Start is called before the first frame update
    void Start()
    {
        _last = Time.realtimeSinceStartup;
    
    }

    float _last=0;
    float delta;
    // Update is called once per frame
    void Update()
    {
        Debug.Log("deltatime: _  "+Time.deltaTime+ "<color=red> __scale::_</color>" + Time.timeScale.ToString()+" _real::  "+delta);//1.游戏时间：time.timescale 和time.delta成正比例  
        if (Input.GetKeyDown(KeyCode.T))
        {
            Time.timeScale = time_Scale;           
        }

        delta= Time.realtimeSinceStartup - _last;//真正实际时间
        _last = Time.realtimeSinceStartup;
        //Debug.Log(_last);
    }
        
}
