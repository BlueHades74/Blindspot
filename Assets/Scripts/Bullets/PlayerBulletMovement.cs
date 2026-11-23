using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBulletMovement : NetworkBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float bulletSpeed = 10f;

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (!IsOwner) return;
        if (context.started) ShootBulletServerRpc(firePoint.position, firePoint.rotation);
    }

    [ServerRpc]
    private void ShootBulletServerRpc(Vector3 spawnPos, Quaternion spawnRot)
    {
        GameObject bullet = Instantiate(bulletPrefab, spawnPos, spawnRot);

        Rigidbody rb = bullet.GetComponent<Rigidbody>();

        if (rb != null) rb.linearVelocity = firePoint.forward * bulletSpeed;

        bullet.GetComponent<NetworkObject>().Spawn(true);

        // old destroy code, only need server to destroy
        //Destroy(bullet, bulletLifetime);
    }
}
