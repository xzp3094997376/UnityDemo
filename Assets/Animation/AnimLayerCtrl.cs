using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimLayerCtrl : MonoBehaviour {
    Animator ani;
    // Start is called before the first frame update
    void Start () {
        transform.localPosition = Vector3.zero;
        ani=GetComponent<Animator>();
        ani.enabled = false;
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.A))
        {
            ani.SetFloat("Offset", 0.2f);                      
        }
    }
}