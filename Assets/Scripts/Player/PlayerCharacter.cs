using SystemScripts;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerCharacter : MonoBehaviour
    {
        public FloatValue lives;
        Rigidbody rb;
        public float moveSpeed = 10f;
        
        // unity event for attack
        public UnityEngine.Events.UnityEvent OnAttack;
        
        [SerializeField] private Item.Item defaultItem;
        
        private Item.Item _currentItem;

        public float healthRegen = 1f;
    


        // unity event for taking damage
        public UnityEngine.Events.UnityEvent OnTakeDamage;
        // slow down value for taking damage
        public SlowDownValue slowDownValueForTakingDamage;
        // sound effect value for taking damage
        public SoundEffectValue soundEffectValueForTakingDamage;

        public HitFlashValue hitFlashValue;

        public FloatValue hitMultiplier;
        
        public DamageNumberValue damageNumberValue;

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
            _currentItem = defaultItem;
            _currentItem.Initialize(this);
        }

        // Update is called once per frame
        void Update()
        {
            hitMultiplier.Value -= Time.deltaTime * healthRegen;
            
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

        public void TakeDamage(Vector3 power,Item.HitData hitData)
        {

            hitMultiplier.Value += hitData.Damage;
            rb.AddForce(power * hitMultiplier.Value);
            TakeDamage();
        }



        public void Action()
        {
            if(_currentItem.InUse)
                return;
            
            OnAttack?.Invoke();
            _currentItem.Use();
            _currentItem.Depleted += LoseItem;
        }

        private void LoseItem()
        {
            _currentItem = defaultItem;
            _currentItem.Initialize(this);
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

        public void PickUpItem(Item.Item item)
        {
            // Todo: ...
            _currentItem = item;
            _currentItem.Initialize(this);
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
}