using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using static UnityEngine.Audio.GeneratorInstance;

public class PlayerDataHandler : NetworkBehaviour
{
    //public static event System.Action<PlayerData> OnPlayerData;
    public static event System.Action<ulong, PlayerData> AnnouncePlayerEntry;
    public static event System.Action<ulong> AnnouncePlayerLeave;

    private Dictionary<ulong, PlayerData> dataStorage = new Dictionary<ulong, PlayerData>();

    private GameObject userInterfaceReader;

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

    private void OnEnable()
    {
        UIReader.AnnounceExistence += GetUI;
    }

    private void OnDisable()
    {
        UIReader.AnnounceExistence -= GetUI;
    }

    public override void OnNetworkSpawn()
    {
        if (IsClient)
        {
            ImportDictionaryRpc();
            FindClientRequestedUser();
        }
        Debug.Log("DATA IS GOING");

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
        string username = userInterfaceReader.GetComponent<UIReader>().ReturnNameField();

        //Send to Rpc
        GeneratePlayerDataRpc(username);
    }

    private void OnClientDisconnect(ulong clientID)
    {
        Debug.Log("Enter");
        //Null check
        if (!dataStorage.ContainsKey(clientID)) return;

        //Do Disconnect Stuff
        Debug.Log("Announce");
        PlayerLeftRpc();

        //Remove from Dictionary
        dataStorage.Remove(clientID);

    }

    private void GetUI(GameObject UI)
    {
        userInterfaceReader = UI;
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

        NewPlayerJoinRpc(clientID, generatedData.ReturnName());
    }

    [Rpc(SendTo.Server)]
    private void ImportDictionaryRpc(RpcParams rpcParams = default)
    {
        if (dataStorage.Count == 0) return;

        foreach (ulong key in dataStorage.Keys)
        {
            GenerateListRpc(key, dataStorage[key].ReturnName(), RpcTarget.Single(rpcParams.Receive.SenderClientId, RpcTargetUse.Temp));
        }
    }

    [Rpc(SendTo.Everyone)]
    private void PlayerLeftRpc(RpcParams rpcParams = default)
    {
        AnnouncePlayerLeave(rpcParams.Receive.SenderClientId);
    }

    [Rpc(SendTo.Everyone)]
    private void NewPlayerJoinRpc(ulong id,string data, RpcParams rpcParams = default)
    {
        var dataCopy = new PlayerData(data);

        AnnouncePlayerEntry.Invoke(id, dataCopy);
    }

    [Rpc(SendTo.SpecifiedInParams)]
    private void GenerateListRpc(ulong id, string data, RpcParams rpcParams = default)
    {
        var dataCopy = new PlayerData(data);

        AnnouncePlayerEntry.Invoke(id, dataCopy);
    }
}
