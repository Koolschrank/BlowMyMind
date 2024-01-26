using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_SplitText : ActionListener
{
    [SerializeField] TextMeshProUGUI[] text_value;

    
    [SerializeField] TextMeshProUGUI[] text_value_max;
    [SerializeField] GameObject text_value_max_parent;
    [SerializeField] bool showZerosInFront = false;



    FloatValue connectedValue;
    [SerializeField] TextInfo textInfo;

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
        float valueFloat = value.Value;
        if (textInfo == TextInfo.value_in_percent)
        {
            valueFloat = (value.Value / value.Value_max)*100;
        }
        // turn to int
        int valueInt = Mathf.RoundToInt(valueFloat);
        // turn to string
        string valueString = valueInt.ToString();
        for (int i = 0; i < text_value.Length; i++)
        {
            // get one digit
            // value sting is to short make it 0
            if (i >= valueString.Length)
            {
                if(showZerosInFront)
                    text_value[i].text = "0";
                else
                    text_value[i].text = "";
            }
            else
            {
                text_value[i].text = valueString[valueString.Length -i-1].ToString();
            }
            

        }

        if (textInfo == TextInfo.value_and_max)
        {
            text_value_max_parent.SetActive(true);
            float valueMaxFloat = value.Value_max;
            // turn to int
            int valueMaxInt = Mathf.RoundToInt(valueMaxFloat);
            // turn to string
            string valueMaxString = valueMaxInt.ToString();
            for (int i = 0; i < text_value.Length; i++)
            {
                // get one digit
                // value sting is to short make it 0
                

                if (i >= valueMaxString.Length)
                {
                    if (showZerosInFront)
                        text_value_max[i].text = "0";
                    else
                        text_value_max[i].text = "";
                }
                else
                {
                    text_value_max[i].text = valueMaxString[valueMaxString.Length - i - 1].ToString();
                }


            }
        }
        else
        {
            text_value_max_parent.SetActive(false);
        }
        

    }

    enum TextInfo
    {
        value,
        value_and_max,
        value_in_percent
    }
}
