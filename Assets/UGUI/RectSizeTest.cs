using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RectSizeTest : MonoBehaviour
{
    public RectTransform rect;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnGUI()
    {
        if (GUI.Button(new Rect(Screen.width-100,0,100,100),"sizeButton"))
        {
           
        }

        GUI.Label(new Rect(Screen.width / 2-200, 100, 100, 100), "sizeDelta:  " + rect.sizeDelta);

        GUI.Label(new Rect(Screen.width / 2, 100, 200, 100), "size:  width=" + rect.rect.width+"  height="+rect.rect.height);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
