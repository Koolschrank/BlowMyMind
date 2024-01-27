using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using VHierarchy.Libs;

public class PlayerCaracter : MonoBehaviour
{
    public FloatValue lives;
    Rigidbody rb;
    public float moveSpeed = 10f;
    public Collider hitBox;
    // layer mask for hitbox
    public LayerMask hitBoxLayerMask;
    // unity event for attack
    public UnityEngine.Events.UnityEvent OnAttack;
    public SlowDownValue slowDownValue;
    public SoundEffectValue soundEffectValue;

    public float baseAttackCooldown = 0.5f;
    float cooldown = 0f;

    public float healthRegen = 1f;
    


    // unity event for taking damage
    public UnityEngine.Events.UnityEvent OnTakeDamage;
    // slow down value for taking damage
    public SlowDownValue slowDownValueForTakingDamage;
    // sound effect value for taking damage
    public SoundEffectValue soundEffectValueForTakingDamage;

    public HitFlashValue hitFlashValue;

    public FloatValue hitMultiplier;
    public HitData BaseDamage;
    public HitData GummiHammerDamage;


    public float federTime=2f;
    float federTimeCounter = 0f;
    public HitData federDamage;
    public DamageNumberValue damageNumberValue;

    public int attackType = 0;

    public float groundedDrag = 1f;
    public float airDrag = 0f;


    public Animator animator;
    public Transform hand;
    public float velocityToWalk = 0.1f;
    bool isGrounded = false;
    public void SetGrounded(bool grounded)
    { 
        if (grounded)
        {
            isGrounded = true;
            Debug.Log("grounded");
            rb.drag = groundedDrag;
        }
        else
        {
            isGrounded = false;
            Debug.Log("not grounded");
            rb.drag = airDrag;
        }
    
    }

    private void Awake()
    {
       // PlayerSystem.instance.AddPlayer(gameObject);
        rb = GetComponent<Rigidbody>();

        hitFlashValue.SetUpHitFlash();

    }

    // Update is called once per frame
    void Update()
    {
        hitMultiplier.Value -= Time.deltaTime * healthRegen;
        
        cooldown -= Time.deltaTime;
        hitFlashValue.UpdateHitFlash(Time.deltaTime);
        MoveUpdate(Time.deltaTime);

    }


    public void MoveUpdate(float delta)
    {
        if (!isGrounded) return;

        Vector3 movementDirection = new Vector3(moveInput.x, 0, moveInput.y).normalized;
        rb.AddForce(movementDirection * movePower * moveSpeed * delta);

        // rotate to face direction of movement
        if (movementDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(movementDirection);
        }
        if (movePower > velocityToWalk)
        {
            animator.SetBool("Walking", true);
        }
        else
        {
            animator.SetBool("Walking", false);
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
            Action();
        }
    }

    public void TakeDamage(Vector3 power,HitData hitData)
    {

        hitMultiplier.Value += hitData.Damage;
        rb.AddForce(power * hitMultiplier.Value);
        TakeDamage();
    }



    public void Action()
    {
        if (cooldown > 0f) return;

        animator.SetTrigger("Attack");
        switch (attackType)
        {
            case 0:
                BaseAttack();
                break;
            case 1:
                FederAttack();
                LoseItem();
                break;
            case 2:
                GummiHammerAttack();
                LoseItem();
                break;
            default:
                break;
        }
        cooldown = baseAttackCooldown;
    }

    public void LoseItem()
    {
        attackType = 0;
    }

    public void BaseAttack()
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
                    Vector3  power = transform.forward * BaseDamage.ForwardForce + transform.up * BaseDamage.UpForce;
                    // take damage
                    playerCaracter.TakeDamage(power, BaseDamage);


                }

            }
        }
    }

    // feder attack
    public void FederAttack()
    {
        // debug
        Debug.Log("feder attack");
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
                    Vector3 power = transform.forward * federDamage.ForwardForce + transform.up * federDamage.UpForce;
                    // take damage
                    playerCaracter.TakeDamage(power, federDamage);
                }
            }
        }
    }

    // gummi hammer attack
    public void GummiHammerAttack()
    {
        // debug
        Debug.Log("gummi hammer attack");
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
                    Vector3 power = transform.forward * GummiHammerDamage.ForwardForce + transform.up * GummiHammerDamage.UpForce;
                    // take damage
                    playerCaracter.TakeDamage(power, GummiHammerDamage);
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
        damageNumberValue.Play(transform);
    }

    public void MoveInput(Vector2 direction)
    {
        moveInput = direction;
        movePower = moveInput.magnitude;
    }

    public void ItemPickUp(int type)
    {
        attackType = type;
    }



    public void Die()
    {
        Debug.Log("Player died!");
        // destroy game object
        PlayerSystem.instance.Respawn(gameObject);
        rb.velocity = Vector3.zero;

        hitMultiplier.Value = 0f;

        lives.Value -= 1f;


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