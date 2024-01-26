using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5;
    Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        MoveUpdate(Time.deltaTime);
    }


    public void MoveUpdate(float delta)
    {

        Vector3 movementDirection = new Vector3(moveInput.x, 0, moveInput.y).normalized;
        rb.velocity = (movementDirection * movePower * moveSpeed);
    }

    Vector2 moveInput;
    float movePower;
    public void MoveInput(InputAction.CallbackContext direction)
    {
        MoveInput(direction.ReadValue<Vector2>());
    }
    public void MoveInput(Vector2 direction)
    {
        moveInput = direction;
        movePower = moveInput.magnitude;
    }
}
