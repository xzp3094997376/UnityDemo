using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class SceneDirty 
{
    // Start is called before the first frame update
    [MenuItem("Tool/sceneDirty")]
    static void Start()
    {       
        EditorSceneManager.SaveScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene());

        EditorSceneManager.SaveScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
