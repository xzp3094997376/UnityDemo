using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TestInput : MonoBehaviour
{
    public Transform tr;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    Transform model;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            model = tr;
        }
        else if (Input.GetMouseButton(0))
        {
            float X = Input.GetAxis("Mouse X");
            float Y = Input.GetAxis("Mouse Y");
                     
            tr.Rotate(transform.TransformDirection(new Vector3(Y*10,-X*10,0)),Space.World);           
          
        }      

        if (model != null)
        {
            float axis = Input.GetAxis("Mouse ScrollWheel");
            if (axis!=0)
            {
                model.localScale += Vector3.one * axis * 10; 
                if (model.localScale.x<0)
                {
                    model.localScale = Vector3.zero;
                }
            }           
            Debug.Log(" " + axis);
        }
    }
}
