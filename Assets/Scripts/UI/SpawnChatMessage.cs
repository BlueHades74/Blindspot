using TMPro;
using UnityEngine;

public class SpawnChatMessage : MonoBehaviour
{
    [SerializeField] private GameObject chatField;
    [SerializeField] private GameObject messagePrefab;

    private void Start()
    {
        if (chatField == null)
        {
            Debug.LogError($"ChatField variable in {name} has not been assigned");
        }
    }

    /*
     * Possible Messages:
     * 
     * Survivors ------
     * Onboarding (Triggered if you spawn as a survivor):
     *  "You are a SURVIVOR"
     *  "SURVIVE the attack"
     *  
     * On SURVIVOR kill:
     *  "Friendly kill"
     *  
     * On INFECTED kill:
     *  "Infected takedown!"
     * 
     * Infected -------
     * Onboarding (Trigger the first time you spawn as infected in the round):
     *  "You are INFECTED"
     *  "KILL all survivors"
     * 
     * On SURVIVOR kill:
     *  "TURNED A SURVIVOR"
     * 
     * Global ----------
     * On SURVIVOR death:
     *  "# Survivors remain"
     * 
     */

    private void AnnounceMessage(string message)
    {
        GameObject createdMessage = Instantiate(messagePrefab, chatField.transform);
        if (!createdMessage.transform.GetChild(0).TryGetComponent(out TextMeshProUGUI txt)) return;
        txt.text = message;
    }

    public void TestMessage()
    {
        string message = "";

        switch (Random.Range(0, 3))
        {
            case 0:
                message = "Turned a survivor";
                break;

            case 1:
                message = "15 survivors remain...";
                break;

            case 2:
                message = "KILL all survivors";
                break;

            default:
                message = "SURVIVE the attack";
                break;
        }

        AnnounceMessage(message);
    }
}
