using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5;
    private Vector2 move;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
        move *= moveSpeed;
        Debug.Log($"Movement values: {move.x}, {move.y}");
    }

    private void FixedUpdate()
    {
        Vector3 moveDirection = (transform.forward * move.y) + (transform.right * move.x);

        rb.linearVelocity = new Vector3(moveDirection.x * moveSpeed, rb.linearVelocity.y, moveDirection.z * moveSpeed);
    }
}
