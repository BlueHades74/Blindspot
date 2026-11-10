using UnityEngine;
using UnityEngine.InputSystem;

public class TurnOnOffFlashlight : MonoBehaviour
{
    [SerializeField] GameObject light;
    private bool lightOn = true;

    public void OnToggleFlashlight(InputAction.CallbackContext context)
    {
        if (context.performed && lightOn)
        {
            light.SetActive(false);
            lightOn = false;
        }
        else if (context.performed)
        {
            light.SetActive(true);
            lightOn = true;
        }
    }
}
