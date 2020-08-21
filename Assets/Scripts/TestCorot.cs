using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCorot : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(A());
    }

    // Update is called once per frame
    IEnumerator A()
    {
        yield return StartCoroutine(B());
        Debug.Log("A");
    }
    IEnumerator B()
    {
        yield return new WaitForEndOfFrame();
        Debug.Log("b1");

        yield return new WaitForSeconds(0.2f);
        Debug.Log("b2");
    }
}
