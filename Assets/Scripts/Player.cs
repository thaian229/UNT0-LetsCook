using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float rotateSpeed = 10f;
    [SerializeField] private GameInput gameInput;

    private bool isWalking = false;

    private void Update()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();

        Vector3 moveDirection = new Vector3(inputVector.x, 0, inputVector.y);
        transform.position += moveDirection * Time.deltaTime * moveSpeed;

        isWalking = moveDirection != Vector3.zero;

        transform.forward = Vector3.Slerp(transform.forward, moveDirection, Time.deltaTime * rotateSpeed);
    }

    public bool IsWalking()
    {
        return isWalking;
    }
}
