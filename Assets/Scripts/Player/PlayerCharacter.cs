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
        public PlayerStats stats;
        public FloatValue lives;
        public Rigidbody rigidBody;
        public float moveSpeed = 10f;
        
        [SerializeField] private Item.Item defaultItem;
        
        public Item.Item _currentItem;

        // unity event for taking damage
        public UnityEngine.Events.UnityEvent OnTakeDamage;
        // slow down value for taking damage
        public SlowDownValue slowDownValueForTakingDamage;
        // sound effect value for taking damage
        public SoundEffectValue soundEffectValueForTakingDamage;

        public HitFlashValue hitFlashValue;

        public FloatValue damage;

        [Header("Visuals")] 
        [SerializeField] private List<Rigidbody> bonesToStun;
        [SerializeField] private SkinnedMeshRenderer bodyMesh;
        public Transform body;
        public bool isFaceRandomized = true;
        public bool isFaceRandomizedOnDeath = true;
        public List<FaceData> availableFaces;
        public DamageNumberValue damageNumberValue;
        [SerializeField] private LaughParticles laughParticles;
        
        public float groundedDrag = 1f;
        public float airDrag = 0f;


        public Animator animator;
        public Transform hand;
        public float velocityToWalk = 0.1f;

        private bool isStunned = false;
        bool isGrounded = false;
        public void SetGrounded(bool grounded)
        { 
            if (grounded)
            {
                isGrounded = true;
                
                rigidBody.drag = groundedDrag;
            }
            else
            {
                isGrounded = false;
                
                rigidBody.drag = airDrag;
            }
        }

        private void Awake()
        {
            damage.Value_max = stats.maxPlayerDamage;
            damage.onValueChange += laughParticles.OnDamageChanged;
            // PlayerSystem.instance.AddPlayer(gameObject);
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
            laughParticles.OnDamageChanged(damage);
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
            if (!isGrounded || isStunned) return;

            Vector3 movementDirection = new Vector3(moveInput.x, 0, moveInput.y).normalized;
            rigidBody.AddForce(movementDirection * movePower * moveSpeed * delta);

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
        
        public void TakeDamage(float amount)
        {
            if (amount <= 0)
                return;
            TakeDamage();
            damage.Value += amount;
        }
        
        public void TakeDamage(Vector3 power, HitData hitData)
        {
            var knockBackMultiplier = stats.GetKnockBackMultiplierByDamage(damage);
            Debug.Log("Damage " + hitData.Damage);
            Debug.Log("Knockback " + knockBackMultiplier);
            var knockBack = power * (knockBackMultiplier + 20.0f);
            if (knockBack != Vector3.zero)
            {
                Stun();
            }
            rigidBody.AddForce(knockBack);
            damage.Value += hitData.Damage;
            TakeDamage();
        }


        public void Stun()
        { 
           animator.enabled = false;
            var stunDuration = 2.0f;//stats.GetStunDurationByDamage(damage);
            if (stunDuration <= 0)
                return;
            foreach (var bone in bonesToStun)
            {
                bone.isKinematic = false;
            }
            isStunned = true;
            Invoke("Unstun", stunDuration);
        }


        public void Unstun()
        {
            foreach (var bone in bonesToStun)
            {
                bone.isKinematic = true;
            }
            isStunned = false;
            //animator.enabled = true;
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
            damage.Value = 0;
            Debug.Log("Player died!");
            // destroy game object
            PlayerSystem.instance.Respawn(gameObject);
            if (isFaceRandomizedOnDeath)
            {
                SetFaceMaterials(availableFaces[Random.Range(0, availableFaces.Count)]);
            }
            rigidBody.velocity = Vector3.zero;
            lives.Value -= 1f;

            PlayerSystem.instance.CheckForWinner();

        }
    }
}