using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TransitionsPlus;

public class SceneSystem : MonoBehaviour
{
   // make singelton and dont destroy on load
    public static SceneSystem instance;
    public GameObject transitionAnimator_start;
    public GameObject transitionAnimator_end;
    public Animator transition;

    int nextSceneIndex;
    private void Awake()
    {
         if (instance == null)
        {
              instance = this;
              DontDestroyOnLoad(gameObject);
         }
         else
        {
              Destroy(gameObject);
         }

        SpawnEndTransition();
    }

    public void SpawnStartTransition()
    {
        GameObject startTransition = Instantiate(transitionAnimator_start);
        startTransition.transform.SetParent(transform);
        startTransition.GetComponent<TransitionAnimator>().onTransitionEnd.AddListener(OnTransitionComplete);
        
        // set active to true
        startTransition.gameObject.SetActive(true);
    }

    public void SpawnEndTransition()
    {
        GameObject endTransition = Instantiate(transitionAnimator_end);
        endTransition.transform.SetParent(transform);
        // set active to true
        endTransition.gameObject.SetActive(true);
    }

    public void ReloadSceneWithTransition()
    {
        LoadSceneWithTransition(SceneManager.GetActiveScene().name);
    }

    public void LoadNextSceneWithTransition()
    {
        LoadSceneByIndexWithTransition(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void LoadSceneByIndexWithTransition(int index)
    {
        nextSceneIndex = index;
        SpawnStartTransition();
    }

    public void LoadSceneWithTransition(string sceneName)
    {

        LoadSceneByIndexWithTransition(SceneManager.GetSceneByName(sceneName).buildIndex);
        
    }

    public void OnTransitionComplete()
    {
        SceneManager.LoadScene(nextSceneIndex);
        
        transitionAnimator_start.gameObject.SetActive(false);
        SpawnEndTransition();

    }

    //public void OnTransitionInComplete()
    //{
    //    SceneManager.LoadScene(nextSceneIndex);
        
    //    transitionAnimator_start.gameObject.SetActive(false);
    //    transitionAnimator_end.gameObject.SetActive(true);
    //}

    public void OnTransitionOutComplete()
    {
        // nothing for now
    }

    // load scene by name
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void LoadSceneByIndex(int index)
    {
        SceneManager.LoadScene(index);
    }

    // load next scene
    public void LoadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
