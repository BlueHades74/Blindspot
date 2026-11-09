using System.Collections.Generic;
using UnityEngine;

struct PlayerData
{
    public string username;

    //Data
    public int gamesWon;
    public int infectedKills;
    public int playersInfected;
}

public class PlayerDataHandler : MonoBehaviour
{
    private Dictionary<ulong, PlayerData> dataStorage = new Dictionary<ulong, PlayerData>();
}
