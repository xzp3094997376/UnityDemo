using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainTextureScale : MonoBehaviour
{
    Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    void Update()
    {
        // Animates main texture scale in a funky way!
        float scaleX = Mathf.Cos(Time.time) * 0.5f + 1;
        float scaleY = Mathf.Sin(Time.time) * 0.5f + 1;
        rend.material.mainTextureScale = new Vector2(scaleX, scaleY);
    }
}
