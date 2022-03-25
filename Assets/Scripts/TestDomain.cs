using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class TestDomain : MonoBehaviour
{
    static int counter = 0;
    private int tt;
    private void Start()
    {
        Debug.Log(counter);
        tt = 0;
        Test(tt);
        Debug.Log(tt);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            counter++;
            Debug.Log("Counter: " + counter);
        }
    }

#if UNITY_2019_OR_NEWR
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
#endif
    static void Init()
    {
        counter = 0;
    }

#if UNITY_2019_OR_NEWR
    [RuntimeInitializeOnLoadMethod]
#endif
    static void RunOnStart()
    {
        Debug.Log("Unregistering quit function");
        Application.quitting -= Quit;
    }

    static void Quit()
    {
        Debug.Log("quit");
    }

    void Test([Out][In] int a)
    {
        a++;
    }

}
