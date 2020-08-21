using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TestRaycast :EventTrigger
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        //GetComponent<Image>()?.color = Color.red;

        Debug.LogError(eventData.pointerCurrentRaycast.gameObject.name);
    }
    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
        Debug.Log(eventData.pointerCurrentRaycast.screenPosition) ;
    }
    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        Debug.Log(eventData.pointerCurrentRaycast.worldPosition);

    }
    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
    }
}
