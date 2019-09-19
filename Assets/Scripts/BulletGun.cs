using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BulletGun : MonoBehaviour
{
    [SerializeField] float baseDamage = 10;
    [HideInInspector] public float damageMultiplier = 1f;

    SimpleBullet simpleBullet;

    [SerializeField] bool isPlayer = true;

    [SerializeField] GameObject simpleBulletPrefab;
    [SerializeField] GameObject spiralBulletPrefab;

    public enum FireShotType { Simple, Diagonal, Spiral };

    public FireShotType m_FireShotType = FireShotType.Simple;    

    private void Awake()
    {
        if(transform.parent.tag == "Player")
        {
            isPlayer = true;
        }else if(transform.parent.tag == "Enemy")
        {
            isPlayer = false;
        }
    }

    public void Shoot()
    {
        switch (m_FireShotType)
        {
            case FireShotType.Simple:
                SimpleShoot(simpleBulletPrefab, Vector3.right);
                break;

            case FireShotType.Diagonal:
                ShootDiagonal(simpleBulletPrefab);
                break;

            case FireShotType.Spiral:
                SimpleShoot(spiralBulletPrefab, Vector3.right);
                break;


            default:
                SimpleShoot(simpleBulletPrefab, Vector3.right);
                break;
        }
    }
    public void SimpleShoot(GameObject bulletPrefab, Vector3 direction)
    {
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

        simpleBullet = bullet.GetComponent<SimpleBullet>();
        simpleBullet.Init(baseDamage * damageMultiplier, direction, 30f, isPlayer);
    }

    public void ShootDiagonal(GameObject bulletPrefab)
    {
        SimpleShoot(bulletPrefab, (Vector3.right + Vector3.up).normalized);
        SimpleShoot(bulletPrefab, (Vector3.right - Vector3.up).normalized);
    }

    public void ShootSpiral(GameObject bulletPrefab)
    {
        SimpleShoot(bulletPrefab, Vector3.right);
    }
}


