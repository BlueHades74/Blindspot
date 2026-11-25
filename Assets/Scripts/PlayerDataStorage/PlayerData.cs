using UnityEngine;

public class PlayerData
{
    private string username;

    //Data
    private int gamesWon;
    private int infectedKills;
    private int playersInfected;

    public PlayerData(string name)
    {
        username = name;
        gamesWon = 0;
        infectedKills = 0;
        playersInfected = 0;
    }

    public string ReturnName()
    { 
        return username; 
    }
}
