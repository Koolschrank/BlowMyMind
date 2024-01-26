using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// scene mmanagement
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{


    public void loadSceneByIndex(int index)
    {
        SceneSystem.instance.LoadSceneByIndexWithTransition(index);
       
    }

    public void loadSceneByName(string name)
    {
        SceneSystem.instance.LoadSceneWithTransition(name);

    }

    public void QuitGame()
    {
        Application.Quit();
    }
    
}
