using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] FloatValue ammunition;
    [SerializeField] float reloadTime = 1;
    float reloadTimer = 0;
    [SerializeField] float fireRate = 0.5f;
    float fireTimer = 0;
    [SerializeField] PoolSpawnValue bullet;
    bool isShooting = false;
    [SerializeField] CameraShakeValue shake;
    [SerializeField] SoundEffectValue shootSound;

    // Start is called before the first frame update
    void Start()
    {
        ammunition.Start();
    }

    // amunition geter setter
    public FloatValue Ammunition
    {
        get { return ammunition; }
        set { ammunition = value; }
    }

    // Update is called once per frame
    void Update()
    {
        fireTimer -= Time.deltaTime;
        reloadTimer -= Time.deltaTime;

        if (reloadTimer <= 0)
        {
            ammunition.AddValue(1);
            reloadTimer = reloadTime;
        }

        if (isShooting && fireTimer <= 0  && ammunition.Value >0)
        {
            Shoot();
        }



        
    }


    public void ShootInput(InputAction.CallbackContext input)
    {
        ShootInput(input.ReadValueAsButton());
    }

    public void ShootInput(bool input)
    {
        isShooting = input;
    }

    public void Shoot()
    {
        shake.Play();
        shootSound.Play();
        ammunition.SubtractValue(1);
        fireTimer = fireRate;

        bullet.Play(transform);
    }
}
