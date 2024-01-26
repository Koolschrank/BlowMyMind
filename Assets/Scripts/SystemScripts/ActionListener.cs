using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class ActionListener: MonoBehaviour
{
    [SerializeField] UnityEvent valueChange;


    public virtual void ConnectValue(FloatValue value)
    {
        return;
    }
    public virtual void UpdateValue(FloatValue value)
    {
        valueChange?.Invoke();
        
    }
}
