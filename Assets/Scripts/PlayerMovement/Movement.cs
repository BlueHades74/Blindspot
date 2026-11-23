using Unity.Cinemachine;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : NetworkBehaviour
{
    [SerializeField] private float moveSpeed = 5;
    private Vector2 move;
    private Rigidbody rb;

    [SerializeField] CinemachineCamera playerCam;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        UpdateStartingPriority();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (!IsOwner) return;
        move = context.ReadValue<Vector2>();
        move *= moveSpeed;
        Debug.Log($"Movement values: {move.x}, {move.y}");
    }

    private void FixedUpdate()
    {
        if (!IsOwner) return;
        Vector3 moveDirection = (transform.forward * move.y) + (transform.right * move.x);

        rb.linearVelocity = new Vector3(moveDirection.x * moveSpeed, rb.linearVelocity.y, moveDirection.z * moveSpeed);
    }

    private void UpdateStartingPriority()
    {
        if (!IsOwner) return;
        // anything above 0 (other cams priority)
        playerCam.Priority = 5; 
    }
}
