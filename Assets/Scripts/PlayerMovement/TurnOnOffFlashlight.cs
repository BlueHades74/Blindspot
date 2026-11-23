using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class TurnOnOffFlashlight : NetworkBehaviour
{
    [SerializeField] GameObject light;
    private NetworkVariable<bool> lightOn = new NetworkVariable<bool>(true);

    public void OnToggleFlashlight(InputAction.CallbackContext context)
    {
        if (!IsOwner) return;

        if (context.performed && lightOn.Value)
        {
            ToggleFlashlightServerRpc(true);
            light.SetActive(false);
        }
        else if (context.performed)
        {
            ToggleFlashlightServerRpc(false);
            light.SetActive(true);
        }
    }

    [ServerRpc]
    private void ToggleFlashlightServerRpc(bool alreadyOn)
    {
        if (alreadyOn)
        {
            light.SetActive(false);
            lightOn.Value = false;
        }
        else
        {
            light.SetActive(true);
            lightOn.Value = true;
        }
    }
}
