using System.Collections;
using System.Collections.Generic;
using SystemScripts;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseTrigger : MonoBehaviour
{
    public void Pause(InputAction.CallbackContext callback)
    {
        if (callback.started)
        {
            if (PlayerSystem.instance.IsGameStarted())
            {
                PauseSystem.instance?.Pause();

            }
            else
            {
                PlayerSystem.instance.StartGame();
            }
        }

       
       
    }
}
