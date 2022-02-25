using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class Test : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public Slider slider;
    private bool isMouseDown = false;

    private void OnEnable()
    {
        isMouseDown = false;
        videoPlayer.frame = 0;
        videoPlayer.Play();
    }

    // Start is called before the first frame update
    void Start()
    {
        videoPlayer.loopPointReached += VideoPlayer_loopPointReached;
        //videoPlayer.frame = 2000;
        Debug.Log(videoPlayer.length);
        slider.onValueChanged.AddListener((process) =>
        {
            if (isMouseDown)
            {
                double curFrame = videoPlayer.length * slider.value * videoPlayer.frameRate;
                videoPlayer.frame = System.Convert.ToInt32(curFrame);
            }
            //transform.Translate();
        });
        
    }

    private void Update()
    {
        if (!isMouseDown)
        {
            slider.value = (float)videoPlayer.frame / videoPlayer.frameCount;
        }
    }

    private void VideoPlayer_loopPointReached(VideoPlayer source)
    {
       Debug.Log(videoPlayer.length);
    }

    public void OnPointerDown()
    {
        isMouseDown = true;
    }


    public void OnPointerUp()
    {
        isMouseDown = false;
    }
    
   
}
