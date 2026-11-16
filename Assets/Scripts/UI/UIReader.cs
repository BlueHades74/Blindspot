using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIReader : MonoBehaviour
{
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
    }

    private void OnDisable()
    {
        usernameInputField.onValueChanged.RemoveListener(InputFieldsUpdated);
        usernameInputField.onValueChanged.RemoveListener(InputFieldsUpdated);
    }

    private void Start()
    {
        UpdateButtons();
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
}
