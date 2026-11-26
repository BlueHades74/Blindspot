using TMPro;
using UnityEngine;

public class PlayerStatsDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerName;

    [SerializeField] private TextMeshProUGUI[] statsEntryFields;

    private ulong clientID;

    private void OnEnable()
    {
        PlayerDataHandler.AnnouncePlayerLeave += PlayerLeft;
    }

    private void OnDisable()
    {
        PlayerDataHandler.AnnouncePlayerLeave -= PlayerLeft;
    }

    private void PlayerLeft(ulong leaverID)
    {
        if (clientID != leaverID) return;
        Destroy(gameObject);
    }

    public void SetName(string name)
    {
        playerName.text = name;
    }

    public void SetStat(int index, string stat)
    {
        statsEntryFields[index].text = stat;
    }

    public void SetStats(ulong id, PlayerData data)
    {
        clientID = id;
        playerName.text = data.ReturnName();
    }
}
