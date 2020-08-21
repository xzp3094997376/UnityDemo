using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRect : MonoBehaviour
{    
    void Start()
    {
        RectTransform rect = GetComponent<RectTransform>();
        rect.offsetMax = new Vector2(-100, -100);
        rect.offsetMin = new Vector2(50, 50);

        Debug.LogError(rect.rect.x+"_"+rect.rect.y);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
