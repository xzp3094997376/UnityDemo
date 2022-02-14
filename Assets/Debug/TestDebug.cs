using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

//[ExecuteInEditMode]
public class TestDebug : MonoBehaviour
{
    [SerializeField]
    int expectedInt = 32;
    [SerializeField]
    int actualInt = 32;
    // Start is called before the first frame update
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        // Do not show message (32 is equal to 32).
        Assert.AreEqual(expectedInt, actualInt, "AreEqual: " + expectedInt + " equals " + actualInt);
        Assert.AreNotEqual(expectedInt, actualInt, "AreNotEqual: " + expectedInt + " not equal to " + actualInt);
    }
}
