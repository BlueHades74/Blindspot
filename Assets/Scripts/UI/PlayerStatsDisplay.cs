using TMPro;
using UnityEngine;

public class PlayerStatsDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerName;

    [SerializeField] private TextMeshProUGUI[] statsEntryFields;

    public void SetName(string name)
    {
        playerName.text = name;
    }

    public void SetStat(int index, string stat)
    {
        statsEntryFields[index].text = stat;
    }
}
