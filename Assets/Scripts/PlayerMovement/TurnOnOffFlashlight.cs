using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class TurnOnOffFlashlight : NetworkBehaviour
{
    [SerializeField] GameObject light;
    private bool lightOn = true;

    public void OnToggleFlashlight(InputAction.CallbackContext context)
    {
        if (!IsOwner) return;

        if (context.performed && lightOn)
        {
            ToggleFlashlightServerRpc(true);
        }
        else if (context.performed)
        {
            ToggleFlashlightServerRpc(false);
        }
    }

    [ServerRpc]
    private void ToggleFlashlightServerRpc(bool alreadyOn)
    {
        if (alreadyOn)
        {
            light.SetActive(false);
            lightOn = false;
        }
        else
        {
            light.SetActive(true);
            lightOn = true;
        }
    }
}
