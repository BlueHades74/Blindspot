using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBulletMovement : NetworkBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float bulletSpeed = 10f;
    [SerializeField] private float bulletLifetime = 3f;

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (!IsOwner) return;
        if (context.started) ShootBulletServerRpc();
    }

    [ServerRpc]
    private void ShootBulletServerRpc()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        bullet.GetComponent<NetworkObject>().Spawn(true);

        Rigidbody rb = bullet.GetComponent<Rigidbody>();

        if (rb != null) rb.linearVelocity = firePoint.forward * bulletSpeed;

        Destroy(bullet, bulletLifetime);
    }
}
