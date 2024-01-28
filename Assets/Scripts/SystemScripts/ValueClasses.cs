using DamageNumbersPro;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[Serializable]
public class PoolSpawnValue
{
    [SerializeField] bool hasSpawn = false;
    [SerializeField] string objectToSpawn = "";
    [SerializeField] bool asChild = false;

    public PoolSpawnValue(string objectToSpawn)
    {
        this.objectToSpawn = objectToSpawn;
    }

    public GameObject Play(Transform transform)
    {
        if (!hasSpawn) return null;

        if (objectToSpawn != "" && ObjectPooler.Instance != null)
        {
            GameObject obj;
            obj = ObjectPooler.Instance.GetObjectAndSetToTransform(objectToSpawn, transform, false);
            if (asChild)
            {
                obj.transform.parent = transform;
            }
            return obj;
        }

        // pritn warning
        Debug.LogWarning("Object to spawn is empty");
        return null;
    }
}

[Serializable]
public class SpawnValue
{
    [SerializeField] bool hasSpawn = false;
    [SerializeField] GameObject objectToSpawn;
    [SerializeField] bool asChild = false;



    public GameObject Play(Transform transform)
    {
        if (!hasSpawn) return null;

        if (objectToSpawn != null && ObjectPooler.Instance != null)
        {
            GameObject obj;
            obj = ObjectPooler.Instance.SpawnObject(objectToSpawn, transform);
            if (asChild)
            {
                obj.transform.parent = transform;
            }
            return obj;
        }

        // pritn warning
        Debug.LogWarning("Object to spawn is empty");
        return null;
    }


}


[Serializable]
public class HitFlashValue
{
    [SerializeField] bool hasHitFlash = false;
    [SerializeField] SkinnedMeshRenderer[] meshes;
    [SerializeField] Material hitMaterial;
    Material[] originalMaterials;
    [SerializeField] float hitFlashTime = 0.1f;
    float hitFlashTimer = 0f;
    bool active = false;

    // setter and getter for hitFlashTimer
    // is active
    public bool Active
    {
        get
        {
            return active;
        }
        set
        {
            active = value;
        }
    }

    public float HitFlashTimer
    {
        get
        {
            return hitFlashTimer;
        }
        set
        {
            hitFlashTimer = value;
        }
    }

    public void UpdateHitFlash(float delta)
    {
        if (active && CheckIfHitFlashIsOver(Time.unscaledDeltaTime))
        {
            EndHitFlash();
        }
    }



    public bool CheckIfHitFlashIsOver(float delta)
    {
        hitFlashTimer -= delta;
        if (hitFlashTimer <= 0)
        {
            active = false;
            return true;
        }
        else
        {
            return false;
        }
    }

    public void SetUpHitFlash()
    {
        // save the original materials
        var originalMaterials = new Material[meshes.Length];
        for (int i = 0; i < meshes.Length; i++)
        {
            originalMaterials[i] = meshes[i].material;
        }
        this.originalMaterials = originalMaterials;
    }

    // setter and getter for original material
    public Material[] OriginalMaterials
    {
        get
        {
            return originalMaterials;
        }
        set
        {
            originalMaterials = value;
        }
    }

    public void StartHitFlash()
    {
        if (!hasHitFlash) return;
        hitFlashTimer = hitFlashTime;
        active = true;
        for (int i = 0; i < meshes.Length; i++)
        {
            meshes[i].material = hitMaterial;
        }
    }

    public void EndHitFlash()
    {
        // restore the original materials
        for (int i = 0; i < meshes.Length; i++)
        {
            meshes[i].material = originalMaterials[i];
        }
    }



}

[Serializable]
public class DamageNumberValue
{
    [SerializeField] bool hasDamageNumber = false;
    [SerializeField] DamageNumber damageNumber;
    [SerializeField] Transform spawnPoint;
    [SerializeField] Vector3 offset;
    [SerializeField] String word;




    public void Play(DamageData damage)
    {
        Play(damage.DamageValue);
    }

    public void Play(Transform transform)
    {
        if (!hasDamageNumber) return;
        damageNumber.Spawn(transform.position + offset, word);
    }

    public void Play(float damage)
    {
        if (!hasDamageNumber) return;
        damageNumber.Spawn(spawnPoint.position + offset, damage);
    }
}

[Serializable]
public class FloatValue
{
    [SerializeField] float value_current = 0f;
    [SerializeField] float value_min = 0f;
    [SerializeField] float value_max = 100f;
    [Tooltip("if true value will be reset to value_max on start")]
    [SerializeField] bool resetOnStart = true;
    [Tooltip("if true value loss event will not be activated when value changes into value_min " +
        "(example: player gets hit and health reaches 0, reather than activating valueLoss and value_Empty, with this activated it would only activate value_empty)")]
    [SerializeField] bool noLossEventOnEmpty = false;
    [Tooltip("if true value gain event will not be activated when value changes into value_max " +
               "(example: player gets healed and health reaches max, reather than activating valueGain and value_Full, with this activated it would only activate value_Full)")]
    [SerializeField] bool noGainEventOnFull = false;
    [SerializeField] UnityEvent valueChange;
    [SerializeField] UnityEvent valueLoss;
    [SerializeField] UnityEvent value_Empty;
    [SerializeField] UnityEvent valueGain;
    [SerializeField] UnityEvent value_Full;
    [SerializeField] List<ActionListener> listeners = new List<ActionListener>();

    public delegate void OnValueChange(FloatValue floatValue);

    // Define an event using the delegate
    public event OnValueChange onValueChange;
    public event OnValueChange onValueLoss;
    public event OnValueChange onValueGain;
    public event OnValueChange onValueFull;
    public event OnValueChange onValueEmpty;

    public void Start()
    {
        if (resetOnStart)
        {
            value_current = value_max; // this should not trigger the valueChange event because it is the start
        }
        foreach (var listener in listeners)
        {
            listener.ConnectValue(this);
        }

    }

    // IsFull and IsEmpty
    public bool IsFull()
    {
        return value_current == value_max;
    }

    public bool IsEmpty()
    {
        return value_current == value_min;
    }

    public void AddListener(ActionListener listener)
    {
        listeners.Add(listener);
        listener.ConnectValue(this);
    }

    public float Value
    {
        get
        {
            return value_current;
        }
        set
        {
            value_current = Mathf.Max(MathF.Min(value, value_max), value_min);

            valueChange?.Invoke();
            onValueChange?.Invoke(this);

            if (value_current == value_min)
            {
                // print
                Debug.Log("Value is empty");

                valueLoss?.Invoke();
                onValueLoss?.Invoke(this);
            }
            
            else if (value_current == value_max)
            {
                value_Full?.Invoke();
                onValueFull?.Invoke(this);
            }
        }
    }

    public float GetPercentage()
    {
        return value_current / value_max;
    }

    // getter and setter for value_min an value_max
    public float Value_min
    {
        get
        {
            return value_min;
        }
        set
        {
            value_min = value;
        }
    }

    public float Value_max
    {
        get
        {
            return value_max;
        }
        set
        {
            value_max = value;
        }
    }

    public void AddValue(float value)
    {
        Value += value;
        if (value > 0 && (!noGainEventOnFull || Value != value_max))
        {
            valueGain?.Invoke();
            onValueGain?.Invoke(this);
        }
        else if (value < 0 && (!noLossEventOnEmpty || Value != value_min))
        {
            valueLoss?.Invoke();
            onValueLoss?.Invoke(this);
        }
    }

    

    public void SubtractValue(float value)
    {
        AddValue(-value);
    }

    public void ResetValue()
    {
        Value = value_max;
    }

}