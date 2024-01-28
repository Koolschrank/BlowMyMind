using System;
using System.Collections.Generic;
using Item;
using SystemScripts;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Player
{
    public class PlayerCharacter : MonoBehaviour
    {
        public Transform body;
        public bool isFaceRandomized = true;
        public bool isFaceRandomizedOnDeath = true;
        public List<FaceData> availableFaces;
        [SerializeField] private SkinnedMeshRenderer bodyMesh;
        public FloatValue lives;
        Rigidbody rb;
        public float moveSpeed = 10f;
        
        [SerializeField] private Item.Item defaultItem;
        
        public Item.Item _currentItem;

        public float healthRegen = 1f;
    


        // unity event for taking damage
        public UnityEngine.Events.UnityEvent OnTakeDamage;
        // slow down value for taking damage
        public SlowDownValue slowDownValueForTakingDamage;
        // sound effect value for taking damage
        public SoundEffectValue soundEffectValueForTakingDamage;

        public HitFlashValue hitFlashValue;

        [FormerlySerializedAs("hitMultiplier")] public FloatValue knockBackMultiplier;
        
        public DamageNumberValue damageNumberValue;
        [SerializeField] private LaughParticles laughParticles;
        
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
                
                rb.drag = groundedDrag;
            }
            else
            {
                isGrounded = false;
                
                rb.drag = airDrag;
            }
        }

        private void Awake()
        {
            knockBackMultiplier.onValueChange += laughParticles.OnDamageChanged;
            // PlayerSystem.instance.AddPlayer(gameObject);
            rb = GetComponent<Rigidbody>();
            hitFlashValue.SetUpHitFlash();
            PickUpItem(defaultItem);

            if (isFaceRandomized)
            {
                SetFaceMaterials(availableFaces[Random.Range(0, availableFaces.Count)]);
            }
        }

        private void SetFaceMaterials(FaceData faceData)
        {
            var materials = bodyMesh.materials;
            materials[0] = faceData.skinMaterial;
            materials[1] = faceData.faceMaterial;
            materials[4] = faceData.hairMaterial;
            bodyMesh.materials = materials;
            Debug.Log("Materials set!");
        }

        public void SetClothsMaterials(Material pantsMaterial, Material shirtMaterial)
        {
            var materials = bodyMesh.materials;
            materials[2] = shirtMaterial;
            materials[3] = pantsMaterial;
            bodyMesh.materials = materials;
        }
        
        private void Start()
        {
            laughParticles.OnDamageChanged(knockBackMultiplier);
        }

        public void EnableItemHitBox()
        {
            _currentItem.EnableHitBox();
        }

        public void DisableItemHitBox()
        {
            _currentItem.DisableHitBox();
        }

        public void ThrowItem()
        {
            _currentItem.Throw();
        }

        public Transform GetBody()
        {
            return body;
        }

        public void SetObjectToHand(Item.Item objectToSet)
        {
            // set object as child of hand
            objectToSet.transform.SetParent(hand);
            // set position to hand
            objectToSet.transform.localPosition = Vector3.zero;
            // set rotation to hand
            objectToSet.transform.localRotation = Quaternion.identity;
        }
        
        // Update is called once per frame
        void Update()
        {
            //knockBackMultiplier.Value -= Time.deltaTime * healthRegen;
            
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
                body.transform.rotation = Quaternion.LookRotation(movementDirection);
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
            Debug.Log("Damage " + hitData.Damage);
            // debug Knockback
            Debug.Log("Knockback " + knockBackMultiplier.Value);
            knockBackMultiplier.Value = knockBackMultiplier.Value + hitData.Damage;
            Debug.Log("Knockback 2" + knockBackMultiplier.Value);
            rb.AddForce(power * knockBackMultiplier.Value);
            TakeDamage();
        }



        public void Action()
        {
            if(_currentItem.InUse)
            {
                // debug
                Debug.Log("Item in use");
                return;
            }
                
            

            _currentItem.Use();
            _currentItem.Depleted += LoseItem;
        }

        public void PlayAttackAnimation()
        {
            animator.SetTrigger("Attack");
        }

        private void LoseItem()
        {
            PickUpItem(defaultItem);
         
        }

        public void TakeDamage()
        {
            
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
            if (_currentItem != null)
            {
                DestroyItem();
            }
            
            // Todo: ...
            // spawn item to hand
            var itemInstance =Instantiate(item, Vector3.zero, Quaternion.identity);
            SetObjectToHand(itemInstance);


            _currentItem = itemInstance;
            _currentItem.Initialize(this);
        }

        // destroy current item
        public void DestroyItem()
        {

            Destroy(_currentItem.gameObject);
            _currentItem = null;
        }

        public void Die()
        {
            knockBackMultiplier.Value -= 1000f;
            Debug.Log("Player died!");
            // destroy game object
            PlayerSystem.instance.Respawn(gameObject);
            if (isFaceRandomizedOnDeath)
            {
                SetFaceMaterials(availableFaces[Random.Range(0, availableFaces.Count)]);
            }
            rb.velocity = Vector3.zero;

            knockBackMultiplier.SubtractValue(100000);
            

            lives.Value -= 1f;

            PlayerSystem.instance.CheckForWinner();

        }
    }
}