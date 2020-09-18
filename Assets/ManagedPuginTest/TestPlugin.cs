using DllTest;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class TestPlugin : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public bool isTest = false;
    // Update is called once per frame
    void Update()
    {
        if (isTest)
        {
            isTest = false;
            print(MyUtilities.GenerateRandom(0, 100));
            Add();
        }
        //Debug.Log("isTest  " + isTest);
    }

    void Add()
    {
        MyUtilities utils = new MyUtilities();
        utils.AddValues(2, 3);
        print("2 + 3 = " + utils.c);
    }
}
