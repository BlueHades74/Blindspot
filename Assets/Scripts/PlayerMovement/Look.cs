using UnityEngine;
using UnityEngine.InputSystem;

public class Look : MonoBehaviour
{
    [SerializeField] private float mouseSensitivity = 12f;
    [SerializeField] private Transform playerBody;

    private Vector2 lookInput;
    private float xRotation = 0f;

    private void Start()
    {
        // lock mouse to center of screen and make it disappear
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        lookInput += context.ReadValue<Vector2>();
    }

    private void LateUpdate()
    {
        float mouseX = lookInput.x * mouseSensitivity * Time.deltaTime;
        float mouseY = lookInput.y * mouseSensitivity * Time.deltaTime;

        // make sure look up and down is not inverted
        xRotation -= mouseY;
        // make sure player cannot do a vertical 360
        xRotation = Mathf.Clamp(xRotation, -70f, 70f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        playerBody.Rotate(Vector3.up * mouseX);

        lookInput = Vector2.zero;
    }
}
