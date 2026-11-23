using Unity.Netcode;
using UnityEngine;

public class AutoDestroy : NetworkBehaviour
{
    [SerializeField] private float timeToDestroy = 2f;

    public override void OnNetworkSpawn()
    {
        if (!IsServer) enabled = false;
    }

    void Update()
    {
        if (!IsServer) return;

        timeToDestroy -= Time.deltaTime;
        if (timeToDestroy <= 0) GetComponent<NetworkObject>().Despawn();
    }
}
