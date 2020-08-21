using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDomain : MonoBehaviour
{
    static int counter = 0;
    private void Start()
    {
        Debug.Log(counter);
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

}
