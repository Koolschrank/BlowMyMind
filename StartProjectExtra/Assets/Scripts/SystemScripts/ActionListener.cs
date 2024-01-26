using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActionListener: MonoBehaviour
{

    public virtual void ConnectValue(FloatValue value)
    {
        return;
    }
    public virtual void UpdateValue(FloatValue value)
    {
        return;
    }
}
