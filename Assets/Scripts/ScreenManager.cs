using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScreenManager : MonoBehaviour
{
    public Button StartButton;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void ChangeScene()
    {
        var currentScene = SceneManager.GetActiveScene();
        if(currentScene == SceneManager.GetSceneByBuildIndex(0))
         SceneManager.LoadScene("IntroScene");
        else if (currentScene == SceneManager.GetSceneByBuildIndex(1))
            SceneManager.LoadScene("PlayScene");
    }
    
    
}
