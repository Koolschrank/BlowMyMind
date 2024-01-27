using System.Collections;
using System.Collections.Generic;
using SystemScripts;
using UnityEngine;

public class PlayerInstance : MonoBehaviour
{
    private void Awake()
    {
        PlayerSystem.instance?.AddPlayer(gameObject);
    }
}
