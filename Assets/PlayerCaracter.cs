using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using VHierarchy.Libs;

public class PlayerCaracter : MonoBehaviour
{
    Rigidbody rb;
    public float moveSpeed = 10f;
    public Collider hitBox;
    // layer mask for hitbox
    public LayerMask hitBoxLayerMask;
    // unity event for attack
    public UnityEngine.Events.UnityEvent OnAttack;
    public SlowDownValue slowDownValue;
    public SoundEffectValue soundEffectValue;


    // unity event for taking damage
    public UnityEngine.Events.UnityEvent OnTakeDamage;
    // slow down value for taking damage
    public SlowDownValue slowDownValueForTakingDamage;
    // sound effect value for taking damage
    public SoundEffectValue soundEffectValueForTakingDamage;

    public HitFlashValue hitFlashValue;

    public float hitMultiplier = 1f;
    public HitData hitData;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        hitFlashValue.SetUpHitFlash();

    }

    // Update is called once per frame
    void Update()
    {
        hitFlashValue.UpdateHitFlash(Time.deltaTime);
        MoveUpdate(Time.deltaTime);
    }


    public void MoveUpdate(float delta)
    {

        Vector3 movementDirection = new Vector3(moveInput.x, 0, moveInput.y).normalized;
        rb.AddForce(movementDirection * movePower * moveSpeed);

        // rotate to face direction of movement
        if (movementDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(movementDirection);
        }
    }

    Vector2 moveInput;
    float movePower;
    public void MoveInput(InputAction.CallbackContext direction)
    {
        MoveInput(direction.ReadValue<Vector2>());
    }


    // attack input
    public void AttackInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Attack();
        }
    }

    public void TakeDamage(Vector3 power,HitData hitData)
    {

        hitMultiplier += hitData.Damage;
        rb.AddForce(power * hitMultiplier);
        TakeDamage();
    }

    public void Attack()
    {
        // debug
        
        // add forece to everything in the hitbox
        Collider[] colliders = Physics.OverlapBox(hitBox.bounds.center, hitBox.bounds.extents, hitBox.transform.rotation, hitBoxLayerMask);
        Debug.Log(colliders.Length);
        foreach (Collider nearbyObject in colliders)
        {

            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if (rb == null)
            {
               continue;
            }
            // if rb is this rb, skip
            if (rb == this.rb)
            {
                continue;
            }
            if (rb != null)
            {
                //rb.AddForce(transform.forward * pushForwadForce);
                //rb.AddForce(transform.up * pushupForce);
                // slow down value
                slowDownValue.Play();
                // sound effect value
                soundEffectValue.Play();
                // unity event in not empty
                if (OnAttack != null)
                {
                    OnAttack.Invoke();
                }
                // if other rb has a playerCharacter component
                PlayerCaracter playerCaracter = rb.GetComponent<PlayerCaracter>();
                if (playerCaracter != null)
                {
                    Vector3  power = transform.forward * hitData.ForwardForce + transform.up * hitData.UpForce;
                    // take damage
                    playerCaracter.TakeDamage(power, hitData);


                }

            }
        }
    }

    public void TakeDamage()
    {
        // slow down value
        slowDownValueForTakingDamage.Play();
        // sound effect value
        soundEffectValueForTakingDamage.Play();
        // unity event in not empty
        if (OnTakeDamage != null)
        {
            OnTakeDamage.Invoke();
        }
        hitFlashValue.StartHitFlash();
    }

    public void MoveInput(Vector2 direction)
    {
        moveInput = direction;
        movePower = moveInput.magnitude;
    }




}


// serializeable class for hit with forwarde force up forece and damage
[System.Serializable]
public class HitData
{
    [SerializeField] float forwardForce = 100f;
    [SerializeField] float upForce = 100f;
    [SerializeField] float damage = 10f;

    public float ForwardForce { get => forwardForce; set => forwardForce = value; }
    public float UpForce { get => upForce; set => upForce = value; }
    public float Damage { get => damage; set => damage = value; }
}