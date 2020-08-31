using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class TextRoll : MonoBehaviour
{
    public Sequence seq;
    public Text text;
    public int old;
    // Start is called before the first frame update
    void Start()
    {
        seq = DOTween.Sequence();
        seq.SetAutoKill(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            TestSeq(indexArr);
        }
    }

    int[] indexArr = new int[] { 10,500};
    void TestSeq(int[] info)
    {
        int newScore = info[1];
        seq.Append(DOTween.To(
        (float value) => {
            float temp= Mathf.Floor(value);
            text.text = temp + "";
        },
        old,
        newScore,
        5f
        ));
    }
}
