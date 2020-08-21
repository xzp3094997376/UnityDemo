    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestProduct : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(Application.dataPath);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Debug.DrawLine(ray.origin, ray.direction * 1000, Color.red, 1000);
        }       
    }
    private string GetProjectName()
    {
        string projectName = string.Empty;

        string[] s = Application.dataPath.Split('/');
        if (s.Length > 1)
        {
            projectName = s[s.Length - 2];
        }

        return projectName;
    }
}
