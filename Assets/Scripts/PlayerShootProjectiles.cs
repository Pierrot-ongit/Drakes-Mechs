using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShootProjectiles : MonoBehaviour
{
    [SerializeField] private Transform pfBullet;
    private void Awake()
    {
        GetComponent<PlayerAimWeapon>().OnShoot += PlayerShootProjectiles_OnShoot;
    }

    private void PlayerShootProjectiles_OnShoot(object sender, PlayerAimWeapon.OnShootEventArgs e)
    {
        // Shoot projectiles.
        Transform bulletTransform = Instantiate(pfBullet, e.weaponEndPointPosition, Quaternion.identity);
        Vector3 shootDir = (e.shootPosition - e.weaponEndPointPosition).normalized;
        bulletTransform.GetComponent<Bullet>().Setup(shootDir);

    }
}
