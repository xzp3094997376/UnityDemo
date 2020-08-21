using UnityEngine;
using System.Runtime.InteropServices;
using System;
using UnityEngine.UI;
using System.Collections;

public class TestFont : MonoBehaviour
{
    public string txt_simple;
    public string txt_tra;
    Text text;
    // Start is called before the first frame update  
    void Start()
    {
        Debug.Log("start");
        
        txt_tra= ChineseStringUtility.ToTraditional(txt_simple);
        text = GetComponent<Text>();
    }

    IEnumerator IEnum()
    {
        yield return new WaitForSeconds(0.1f);
    }
    // Update is called once per frame 
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            text.text = ChineseStringUtility.ToSimplified(txt_tra);
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            text.text = ChineseStringUtility.ToTraditional(txt_simple);
        }
    }
}



public static class ChineseStringUtility
{
    private const int LOCALE_SYSTEM_DEFAULT = 0x0800;
    private const int LCMAP_SIMPLIFIED_CHINESE = 0x02000000;
    private const int LCMAP_TRADITIONAL_CHINESE = 0x04000000;

    [DllImport("kernel32", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern int LCMapString(int Locale, int dwMapFlags, string lpSrcStr, int cchSrc, [Out] string lpDestStr, int cchDest);

    /// <summary>
    /// 讲字符转换为繁体中文
    /// </summary>
    /// <param name="source">输入要转换的字符串</param>
    /// <returns>转换完成后的字符串</returns>
    public static string ToTraditional(string source)
    {
        String target = new String(' ', source.Length);
        int ret = LCMapString(LOCALE_SYSTEM_DEFAULT, LCMAP_TRADITIONAL_CHINESE, source, source.Length, target, source.Length);
        return target;
    }
    public static string ToSimplified(string source)
    {
        String target = new String(' ', source.Length);
        int ret = LCMapString(LOCALE_SYSTEM_DEFAULT, LCMAP_SIMPLIFIED_CHINESE, source, source.Length, target, source.Length);
        return target;
    }
}