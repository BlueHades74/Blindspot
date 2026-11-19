using Unity.Netcode;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using Unity.Services.Authentication;
using UnityEngine;
using System.Threading.Tasks;
using System;
using Unity.Netcode.Transports.UTP;

public class ConnectionManager : MonoBehaviour
{
    public static event Action<string> announceMessage;
    public static ConnectionManager singletonInstance { get; private set; }

    private bool isBusy = false;
    private string joinCode;

    private GameObject userInterfaceReader;

    private void Awake()
    {
        if (singletonInstance != null && singletonInstance != this) Destroy(gameObject);

        singletonInstance = this;
    }

    private void Start()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
        NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnected;
        ClearConnection();
    }

    private void OnEnable()
    {
        UIReader.AnnounceExistence += GetUI;
    }

    private void OnDisable()
    {
        UIReader.AnnounceExistence -= GetUI;
    }

    private void OnDestroy()
    {
        if (NetworkManager.Singleton == null) return;

        NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
        NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnected;
    }

    public async void Host()
    {
        if (isBusy || NetworkManager.Singleton.IsListening) return;
        isBusy = true;

        try
        {
            announceMessage.Invoke("Connecting...");
            await InitializeUnityServices();
            Allocation alloc = await RelayService.Instance.CreateAllocationAsync(15); //-----------------------------------------------------
            UnityTransport transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
            transport.SetRelayServerData(AllocationUtils.ToRelayServerData(alloc, "dtls"));
            joinCode = await RelayService.Instance.GetJoinCodeAsync(alloc.AllocationId);

            if (NetworkManager.Singleton.StartHost())
            {

            }
            else
            {
                ClearConnection();
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Host failed in Connection Manager. Message: " + e.Message);
            announceMessage.Invoke("Host Failed");
        }
        finally
        {
            isBusy = false;
        }
    }

    public async void Join()
    {
        if (isBusy || NetworkManager.Singleton.IsListening) return;
        isBusy = true;

        try
        {
            announceMessage.Invoke("Connecting...");

            joinCode = userInterfaceReader.GetComponent<UIReader>().ReturnJoinField();

            await InitializeUnityServices();
            JoinAllocation alloc = await RelayService.Instance.JoinAllocationAsync(joinCode);
            UnityTransport transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
            transport.SetRelayServerData(AllocationUtils.ToRelayServerData(alloc, "dtls"));

            if (NetworkManager.Singleton.StartClient())
            {

            }
            else
            {
                ClearConnection();
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Join failed in Connection Manager. Message: " + e.Message);
            announceMessage.Invoke("Join Failed");
        }
        finally
        {
            isBusy = false;
        }
    }

    public async Task InitializeUnityServices()
    {
        try
        {
            if (UnityServices.State != ServicesInitializationState.Initialized)
            {
                await UnityServices.InitializeAsync();
            }

            if (AuthenticationService.Instance.IsSignedIn) return;
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
        catch (Exception e)
        {
            Debug.LogError("Initialization Failed in Connection Manager. Message: " + e.Message);
            announceMessage.Invoke("Initialize Failed");

            throw;
        }
    }

    private void OnClientConnected(ulong clientId)
    {
        if (clientId != NetworkManager.Singleton.LocalClientId) return;
    }

    private void OnClientDisconnected(ulong clientId)
    {
        if (clientId != NetworkManager.Singleton.LocalClientId && !NetworkManager.Singleton.IsServer) ClearConnection();
    }

    private void ClearConnection()
    {
        if (NetworkManager.Singleton.IsListening) NetworkManager.Singleton.Shutdown();
    }

    private void GetUI(GameObject UI)
    {
        userInterfaceReader = UI;
    }
}
