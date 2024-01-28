using System.Collections;
using UnityEngine;

public class AppearanceChange : MonoBehaviour
{
    [SerializeField] private float changeTime;

    private WaitForSeconds _changeDelay;
    
    void Start()
    {
        _changeDelay = new WaitForSeconds(changeTime);
        StartCoroutine(ContinousChange());
    }

    private IEnumerator ContinousChange()
    {
        while (true)
        {
            // Todo: Add material change here
            yield return _changeDelay;
        }
    }
}
