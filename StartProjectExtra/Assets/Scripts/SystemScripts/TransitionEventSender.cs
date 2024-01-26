using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionEventSender : MonoBehaviour
{
    public void OnTransitionInComplete()
    {
        SceneSystem.instance.OnTransitionComplete();
    }

    public void OnTransitionOutComplete()
    {
        SceneSystem.instance.OnTransitionOutComplete();
    }
}
