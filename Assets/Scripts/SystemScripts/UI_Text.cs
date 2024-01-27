using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_Text : ActionListener
{
    TextMeshProUGUI text;
    FloatValue connectedValue;
    [SerializeField] TextInfo textInfo;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
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

    float lastValue = -1;
    public override void UpdateValue(FloatValue value)
    {
        if (text == null) { text = GetComponent<TextMeshProUGUI>(); }
        switch (textInfo)
        {
            case TextInfo.value:
                //text.text = value.Value.ToString();
                // show value as int 
                text.text = Mathf.RoundToInt(value.Value).ToString();
                break;
            case TextInfo.value_and_max:
                text.text = value.Value.ToString() + " / " + value.Value_max.ToString();
                break;
            case TextInfo.value_and_min_max:
                text.text = value.Value_min.ToString() + " / " + value.Value.ToString() + " / " + value.Value_max.ToString();
                break;
            default:
                break;
        }
        if (lastValue < value.Value)
        {
            base.UpdateValue(value);
        }

        
        lastValue = value.Value;
        
    }

    enum TextInfo
    {
        value,
        value_and_max,
        value_and_min_max,
    }
}
