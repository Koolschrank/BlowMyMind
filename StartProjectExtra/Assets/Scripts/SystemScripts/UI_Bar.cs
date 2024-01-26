using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Bar : ActionListener
{
    Slider slider;
    FloatValue connectedValue;

    private void Awake()
    {
        slider = GetComponent<Slider>();
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
        if (slider  == null) { slider = GetComponent<Slider>(); }
       
        slider.maxValue = value.Value_max;
        slider.minValue = value.Value_min;
        slider.value = value.Value;
    }
}
