using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake instance;
    [SerializeField] CameraShakeValue defaultShakeValue;
    [SerializeField] bool debugMode = false;
    float debugTimer = 0f;
    List<CameraShakeValue_inAction> cameraShakeValue_InActions = new List<CameraShakeValue_inAction>();
    
    private void Awake()
    {
        instance = this;
    }

    public void ShakeCamera()
    {
        cameraShakeValue_InActions.Add(new CameraShakeValue_inAction(defaultShakeValue));
    }

    public void ShakeCamera(CameraShakeValue shakeValue)
    {
        cameraShakeValue_InActions.Add(new CameraShakeValue_inAction(shakeValue));
    }


    private void Update()
    {
        if (TimeManager.instance != null && TimeManager.instance.IsPaused())
        {
            return;
        }


        if (debugMode)
        {
            DebugUpdate(Time.deltaTime); 
        }

        if (cameraShakeValue_InActions.Count > 0)
        {
            float strongestShake = GetStrongestShake(Time.unscaledDeltaTime);
            if (strongestShake > 0)
            {
                transform.localPosition = Random.insideUnitSphere * strongestShake;
            }
            else
            {
                transform.localPosition = Vector3.zero;
            }
        }
    }

    public void DebugUpdate(float delta)
    {
        if (debugTimer > 0)
        {
            debugTimer -= delta;
        }
        else
        {
            ShakeCamera();
            debugTimer = 1f;
        }
        
    }

    public float GetStrongestShake(float delta)
    {
        float strongestShake = 0f;
        List<CameraShakeValue_inAction> actionsToRemove = new List<CameraShakeValue_inAction>();

        foreach (CameraShakeValue_inAction action in cameraShakeValue_InActions)
        {
            if (action.CheckIfOver(delta))
            {
                actionsToRemove.Add(action);
            }
            else if (action.GetShakeValue() > strongestShake)
            {
                strongestShake = action.GetShakeValue();
            }
        }

        foreach (CameraShakeValue_inAction action in actionsToRemove)
        {
            cameraShakeValue_InActions.Remove(action);
        }


        return strongestShake;
    }
    


}

public class CameraShakeValue_inAction
{
    float timer = 0f;
    CameraShakeValue cameraShakeValue;

    public CameraShakeValue_inAction(CameraShakeValue shakeValue)
    {
        cameraShakeValue = shakeValue;
    }

    public bool CheckIfOver(float delta)
    {
        timer += delta;
        return CheckIfOver();
    }

    public bool CheckIfOver()
    {
        return cameraShakeValue.CheckIfOver(timer);
    }

    public float GetShakeValue()
    {
        return cameraShakeValue.GetShakeValue(timer);
    }
}


//camera shake value class that can be used to shake the camera
[System.Serializable]
public class CameraShakeValue
{
    [SerializeField] bool hasShake = false;
    [SerializeField] float duration = 0.5f;
    [SerializeField] float magnitude = 0.1f;
    [Tooltip("0 is no shake and 1 is shake equal to magnitude")]
    [SerializeField] AnimationCurve magnitudeCurve;

    // getter for duration

    public bool CheckIfOver(float timer)
    {
        return timer > duration;
    }

    public void Play()
    {
        if (!hasShake) return;
        CameraShake.instance?.ShakeCamera(this);
    }

    public float GetShakeValue(float timer)
    {
        return magnitude * magnitudeCurve.Evaluate(timer / duration);
    }
}



