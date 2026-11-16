using Unity.Netcode;
using Unity.Services.Core;
//using Unity.Services.Relay;
//using Unity.Services.Authentication;
using UnityEngine;
using System.Threading.Tasks;
using System;

public class ConnectionManager : MonoBehaviour
{
    public static event Action<string> announceMessage;
    public static ConnectionManager singletonInstance { get; private set; }

    private bool isBusy = false;
    private string joinCode;

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

    public async Task InitializeUnityServices()
    {
        try
        {
            if (UnityServices.State != ServicesInitializationState.Initialized)
            {
                await UnityServices.InitializeAsync();
            }

            //if (AuthenticationService.Instance.IsSignedIn) return;
            //await AuthenticationService.Instance.SignInAnonymouslyAsync();
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
}
