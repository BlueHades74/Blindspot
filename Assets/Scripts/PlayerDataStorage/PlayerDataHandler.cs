using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

struct PlayerData
{
    public string username;

    //Data
    public int gamesWon;
    public int infectedKills;
    public int playersInfected;

    public PlayerData(string name)
    {
        username = name;
        gamesWon = 0;
        infectedKills = 0;
        playersInfected = 0;
    }
}

public class PlayerDataHandler : NetworkBehaviour
{
    private Dictionary<ulong, PlayerData> dataStorage = new Dictionary<ulong, PlayerData>();

    private void Awake()
    {
        /*
        //Verify that this is a singleton
        if (FindAnyObjectByType<PlayerDataHandler>())
        {
            Destroy(gameObject);
        }

        //Set Don't Destroy On Load
        DontDestroyOnLoad(gameObject);
        */
    }

    public override void OnNetworkSpawn()
    {
        if (IsClient)
        {
            FindClientRequestedUser();
        }

        if (IsServer) NetworkManager.OnClientDisconnectCallback += OnClientDisconnect;
        
        base.OnNetworkSpawn();
    }

    public override void OnNetworkDespawn()
    {
        if (IsServer) NetworkManager.OnClientDisconnectCallback -= OnClientDisconnect;

        base.OnNetworkDespawn();
    }

    private void FindClientRequestedUser()
    {
        //Retrieve Username From Input Field
        string username = "";

        //Send to Rpc
        GeneratePlayerDataRpc(username);
    }

    private void OnClientDisconnect(ulong clientID)
    {
        //Null check
        if (!dataStorage.ContainsKey(clientID)) return;

        //Do Disconnect Stuff


        //Remove from Dictionary
        dataStorage.Remove(clientID);

    }


    //RPCs 
    [Rpc(SendTo.Server)]
    private void GeneratePlayerDataRpc(string username, RpcParams rpcParams = default)
    {
        ulong clientID = rpcParams.Receive.SenderClientId;

        username = username.Trim();

        if (string.IsNullOrEmpty(username) || string.IsNullOrWhiteSpace(username)) username = "Player " + (dataStorage.Count + 1).ToString();

        PlayerData generatedData = new PlayerData(username);
        dataStorage.Add(clientID, generatedData);
    }
}
