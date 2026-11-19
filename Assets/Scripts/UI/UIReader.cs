using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIReader : MonoBehaviour
{
    public static event System.Action<GameObject> AnnounceExistence;

    [Header("Username Screen")]

    [Header("InputFields")]
    [SerializeField] private TMP_InputField usernameInputField;
    [SerializeField] private TMP_InputField joinCodeInputField;

    [Header("Buttons")]
    [SerializeField] private Button[] onlyUsernameBtns;
    [SerializeField] private Button[] bothFieldBtns;

    [Header("----------------------------------------------------")]

    [Header("Player List Screen")]

    [Header("GameObjects")]
    [SerializeField] private GameObject playerListParent;
    [SerializeField] private GameObject statlineContainer;


    [Header("Prefabs")]
    [SerializeField] private GameObject statlinePrefab;

    private void OnEnable()
    {
        usernameInputField.onValueChanged.AddListener(InputFieldsUpdated);
        joinCodeInputField.onValueChanged.AddListener(InputFieldsUpdated);

        ConnectionManager.announceMessage += AcceptMessage;
        PlayerDataHandler.AnnouncePlayerEntry += AddStatline;
    }

    private void OnDisable()
    {
        usernameInputField.onValueChanged.RemoveListener(InputFieldsUpdated);
        joinCodeInputField.onValueChanged.RemoveListener(InputFieldsUpdated);

        ConnectionManager.announceMessage -= AcceptMessage;
        PlayerDataHandler.AnnouncePlayerEntry -= AddStatline;
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

    private void AddStatline()
    {

    }

    public string ReturnJoinField()
    {
        return joinCodeInputField.text;
    }
}
