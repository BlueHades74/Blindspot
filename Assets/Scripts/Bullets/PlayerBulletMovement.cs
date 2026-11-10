using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBulletMovement : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float bulletSpeed = 10f;
    [SerializeField] private float bulletLifetime = 3f;

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (context.started) ShootBullet();
    }

    private void ShootBullet()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        Rigidbody rb = bullet.GetComponent<Rigidbody>();

        if (rb != null) rb.linearVelocity = firePoint.forward * bulletSpeed;

        Destroy(bullet, bulletLifetime);
    }
}
