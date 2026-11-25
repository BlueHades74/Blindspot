using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using static UnityEngine.Audio.GeneratorInstance;

public class PlayerDataHandler : NetworkBehaviour
{
    //public static event System.Action<PlayerData> OnPlayerData;
    public static event System.Action<string> AnnouncePlayerEntry;

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
        //Null check
        if (!dataStorage.ContainsKey(clientID)) return;

        //Do Disconnect Stuff


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

        NewPlayerJoinRpc(generatedData.ReturnName());
    }

    [Rpc(SendTo.Server)]
    private void ImportDictionaryRpc(RpcParams rpcParams = default)
    {
        if (dataStorage.Count == 0) return;

        foreach (PlayerData data in dataStorage.Values)
        {
            GenerateListRpc(data.ReturnName(), RpcTarget.Single(rpcParams.Receive.SenderClientId, RpcTargetUse.Temp));
        }
    }

    [Rpc(SendTo.Everyone)]
    private void NewPlayerJoinRpc(string data, RpcParams rpcParams = default)
    {
        AnnouncePlayerEntry.Invoke(data);
    }

    [Rpc(SendTo.SpecifiedInParams)]
    private void GenerateListRpc(string data, RpcParams rpcParams = default)
    {
        AnnouncePlayerEntry.Invoke(data);
    }
}
