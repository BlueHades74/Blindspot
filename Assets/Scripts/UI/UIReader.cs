using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIReader : MonoBehaviour
{
    public static event System.Action<GameObject> AnnounceExistence;

    [Header("InputFields")]
    [SerializeField] TMP_InputField usernameInputField;
    [SerializeField] TMP_InputField joinCodeInputField;

    [Header("Buttons")]
    [SerializeField] Button[] onlyUsernameBtns;
    [SerializeField] Button[] bothFieldBtns;

    private void OnEnable()
    {
        usernameInputField.onValueChanged.AddListener(InputFieldsUpdated);
        joinCodeInputField.onValueChanged.AddListener(InputFieldsUpdated);

        ConnectionManager.announceMessage += AcceptMessage;
    }

    private void OnDisable()
    {
        usernameInputField.onValueChanged.RemoveListener(InputFieldsUpdated);
        joinCodeInputField.onValueChanged.RemoveListener(InputFieldsUpdated);

        ConnectionManager.announceMessage -= AcceptMessage;
    }

    private void Start()
    {
        UpdateButtons();
        AnnounceExistence.Invoke(gameObject);
    }

    private void AcceptMessage(string message)
    {
        GetComponent<SpawnChatMessage>().AnnounceMessage(message);
    }

    private void InputFieldsUpdated(string val)
    {
        UpdateButtons();
    }

    private void UpdateButtons()
    {
        for (int i = 0; i < onlyUsernameBtns.Length; i++)
        {
            onlyUsernameBtns[i].interactable = HasValidInput(usernameInputField);
        }

        for (int i = 0; i < bothFieldBtns.Length; i++)
        {
            bothFieldBtns[i].interactable = (HasValidInput(usernameInputField) && HasValidInput(joinCodeInputField));
        }
    }

    private bool HasValidInput(TMP_InputField inputField)
    {
        if (string.IsNullOrEmpty(inputField.text)) return false;
        if (string.IsNullOrWhiteSpace(inputField.text)) return false;

        return true;
    }

    public string ReturnJoinField()
    {
        return joinCodeInputField.text;
    }
}
