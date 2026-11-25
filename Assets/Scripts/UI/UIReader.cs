using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIReader : MonoBehaviour
{
    public static event System.Action<GameObject> AnnounceExistence;

    [Header("Username Screen")]
    [SerializeField] private GameObject userScreenParent;

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
    [SerializeField] private TextMeshProUGUI joinCode;


    [Header("Prefabs")]
    [SerializeField] private GameObject statlinePrefab;

    private void OnEnable()
    {
        usernameInputField.onValueChanged.AddListener(InputFieldsUpdated);
        joinCodeInputField.onValueChanged.AddListener(InputFieldsUpdated);

        ConnectionManager.announceMessage += AcceptMessage;
        ConnectionManager.AnnounceConnection += SwitchToRoom;

        PlayerDataHandler.AnnouncePlayerEntry += AddStatline;
    }

    private void OnDisable()
    {
        usernameInputField.onValueChanged.RemoveListener(InputFieldsUpdated);
        joinCodeInputField.onValueChanged.RemoveListener(InputFieldsUpdated);

        ConnectionManager.announceMessage -= AcceptMessage;
        ConnectionManager.AnnounceConnection -= SwitchToRoom;

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

    private void AddStatline(string data)
    {
        GameObject display = Instantiate(statlinePrefab, statlineContainer.transform);
        display.GetComponent<PlayerStatsDisplay>().SetName(data);
    }

    private void SwitchToRoom(string text)
    {
        userScreenParent.SetActive(false);
        playerListParent.SetActive(true);
        
        joinCode.text = text;
    }

    public string ReturnNameField()
    {
        return usernameInputField.text;
    }

    public string ReturnJoinField()
    {
        return joinCodeInputField.text;
    }
}
