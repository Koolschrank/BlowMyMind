using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Points : ActionListener
{
    public Image[] ui_Points;
    FloatValue connectedValue;

    private void Awake()
    {
        
        if (connectedValue == null)
            gameObject.SetActive(false);
    }

    public override void ConnectValue(FloatValue floatValue)
    {
        // set all children to active
        gameObject.SetActive(true);


        connectedValue = floatValue;
        connectedValue.onValueChange += UpdateValue;
        UpdateValue(floatValue);
    }

    public override void UpdateValue(FloatValue value)
    {
        for (int i = 0; i < ui_Points.Length; i++)
        {
            if (i < Mathf.Min((int)value.Value, ui_Points.Length))
            {
                ui_Points[i].enabled = true;
            }
            else
            {
                ui_Points[i].enabled = false;
            }
        }
        base.UpdateValue(value);
    }
}
