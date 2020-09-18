using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Base : MonoBehaviour
{
    public virtual void Test()
    {
        Debug.Log(this.name);
    }
    
    public void Update()
    {
        GetComponent<Base>().Test();
    }
}
