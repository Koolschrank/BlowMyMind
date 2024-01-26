using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseTrigger : MonoBehaviour
{
    public void Pause(InputAction.CallbackContext callback)
    {
        PauseSystem.instance?.Pause();
    }
}
