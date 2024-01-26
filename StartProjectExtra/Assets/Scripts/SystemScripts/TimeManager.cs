using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager instance;
    [SerializeField] float baseTimeScale = 1f;

    [SerializeField] SlowDownValue defaultTimeValue;
    [SerializeField] bool debugMode = false;
    float debugTimer = 0f;


    List<SlowDownValue_inAction> slowDownValue_InActions = new List<SlowDownValue_inAction>();

    private void Awake()
    {
        instance = this;
        Time.timeScale = baseTimeScale ;
    }


    


    private void Update()
    {
        if (IsPaused()) return;
        
        if (slowDownValue_InActions.Count > 0)
        {
            UpdateSlowDown(Time.unscaledDeltaTime);
        }

        if (debugMode)
        {
            DebugUpdate(Time.deltaTime);

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
            SlowDownTime();
            debugTimer = 1f;
        }
        
    }

    public void UpdateSlowDown(float delta)
    {

        float timeScale = baseTimeScale;
        List<SlowDownValue_inAction> actionsToRemove = new List<SlowDownValue_inAction>();
        foreach (SlowDownValue_inAction action in slowDownValue_InActions)
        {

            if (action.CheckIfOver(delta))
            {
                actionsToRemove.Add(action);
            }
            else if (action.GetTimeScale() < timeScale)
            {
                timeScale = action.GetTimeScale();
            }

        }


        foreach (SlowDownValue_inAction action in actionsToRemove)
        {
            slowDownValue_InActions.Remove(action);
        }
        Time.timeScale = timeScale;
    }

    public void SlowDownTime()
    {
        slowDownValue_InActions.Add(new SlowDownValue_inAction(defaultTimeValue));
    }



    public void SlowDownTime(SlowDownValue slowDownValue)
    {
        slowDownValue_InActions.Add(new SlowDownValue_inAction(slowDownValue));

    }



    bool isPaused = false;
    public void PauseGame()
    {
        Time.timeScale = 0f;
        isPaused = true;

    }

    public void ResumeGame()
    {
        Time.timeScale = baseTimeScale;
        isPaused = false;
    }

    public bool IsPaused()
    {
        return isPaused;
    }

}

public class SlowDownValue_inAction
{
    SlowDownValue slowDownValue;
    float timer = 0f;
    public SlowDownValue_inAction(SlowDownValue slowDownValue)
    {
        this.slowDownValue = slowDownValue;
    }

    public bool CheckIfOver(float delta)
    {
        timer += delta;
        return CheckIfOver();
    }

    public bool CheckIfOver()
    {
        return timer > slowDownValue.Duration;
    }

    public float GetTimeScale()
    {
        return slowDownValue.MagnitudeCurve.Evaluate(timer / slowDownValue.Duration);
    }
}

[System.Serializable]
public class SlowDownValue
{
    [SerializeField] bool hasSlowDown = false;
    [Tooltip("0 is 0 Time and 1 is baseTimeScale")]
    [SerializeField] AnimationCurve magnitudeCurve;
    [SerializeField] float duration = 0.5f;

    // getter for duration and magnitudeCurve
    public float Duration
    {
        get { return duration; }
    }

    public AnimationCurve MagnitudeCurve
    {
        get { return magnitudeCurve; }
    }

    public void Play()
    {
        if (!hasSlowDown) return;
        TimeManager.instance?.SlowDownTime(this);
    }
}


