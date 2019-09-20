﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum FireShotType { Simple, Diagonal, Spiral };
public class BulletGun : MonoBehaviour
{
    [SerializeField] float baseDamage = 10;
    [HideInInspector] public float damageMultiplier = 1f;

    SimpleBullet simpleBullet;
    Bullet bulletScript;

    [SerializeField] bool isPlayer = true;

    [SerializeField] GameObject simpleBulletPrefab;
    [SerializeField] GameObject spiralBulletPrefab;

    

    public FireShotType m_FireShotType = FireShotType.Simple;

    [Space]
    [SerializeField] float simpleShotEnergyAmount = 10f;
    [SerializeField] float diagonalShotEnergyAmount = 20f;
    [SerializeField] float spiralShotEnergyAmount = 30f;

    [Space]
    [SerializeField] Energy energy;

    [Space]
    [Header("BulletSelectionUIImage")]
    public float alphaValueWhenNotActive = 0.5f;
    public Image[] bulletImageSelection;

    [Space]
    [Header("Pool Factory")]
    Factory factory;

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

    private void Start()
    {
        factory = Factory.m_Factory;
    }

    public void Shoot()
    {
        switch (m_FireShotType)
        {
            case FireShotType.Simple:
                SimpleShoot(simpleBulletPrefab, (isPlayer ? Vector3.right : Vector3.left));
                energy.SpendEnergy(simpleShotEnergyAmount);
                break;

            case FireShotType.Diagonal:
                ShootDiagonal(simpleBulletPrefab);
                energy.SpendEnergy(diagonalShotEnergyAmount);
                break;

            case FireShotType.Spiral:
                ShootSpiral(spiralBulletPrefab);
                //SimpleShoot(spiralBulletPrefab, (isPlayer ? Vector3.right : Vector3.left));
                energy.SpendEnergy(spiralShotEnergyAmount);
                break;


            default:
                SimpleShoot(simpleBulletPrefab, (isPlayer ? Vector3.right : Vector3.left));
                break;
        }
    }
    public void SimpleShoot(GameObject bulletPrefab, Vector3 direction, bool isSpiralBullet = false)
    {
        //Debug.Log("isSpiralBullet: " + isSpiralBullet);
        //GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        GameObject bullet = factory.CreateBullet(transform.position, Quaternion.identity, isSpiralBullet);

        //simpleBullet = bullet.GetComponent<SimpleBullet>();
        //simpleBullet.Init(baseDamage * damageMultiplier, direction, 30f, isPlayer);
        bulletScript = bullet.GetComponent<Bullet>();
        bulletScript.Init(baseDamage * damageMultiplier, direction, 30f, isPlayer, isSpiralBullet);
    }

    public void ShootDiagonal(GameObject bulletPrefab)
    {
        SimpleShoot(bulletPrefab, ((isPlayer ? Vector3.right : Vector3.left) + Vector3.up).normalized);
        SimpleShoot(bulletPrefab, ((isPlayer ? Vector3.right : Vector3.left) - Vector3.up).normalized);
        SimpleShoot(bulletPrefab, (isPlayer ? Vector3.right : Vector3.left));
    }

    public void ShootSpiral(GameObject bulletPrefab)
    {
        SimpleShoot(bulletPrefab, (isPlayer ? Vector3.right : Vector3.left), true);
    }
}


