using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class InputManager : MonoBehaviour
{
    [SerializeField] MoveShip playerMoveShip;
    [SerializeField] BulletGun playerBulletGun;
    //[SerializeField] GameObject simpleBulletPrefab;

    [SerializeField] float timeBetweenShots = 0.2f;
    float timeSinceLastShot;

    [SerializeField] float timeBetweenChangingShotType = 0.2f;
    float timeSinceChangeOfShotType;

    private void Awake()
    {
        timeSinceLastShot = timeBetweenShots;

        timeSinceChangeOfShotType = timeBetweenChangingShotType;
    }

    void Update()
    {
        float w = CrossPlatformInputManager.GetAxis("Vertical");
        float h = CrossPlatformInputManager.GetAxis("Horizontal");

        float fire = CrossPlatformInputManager.GetAxis("Jump");

        playerMoveShip.move = (new Vector3(h, w, 0f)).normalized;



        timeSinceLastShot += Time.deltaTime;
        if (fire >= 0.5f && timeSinceLastShot >= timeBetweenShots)
        {
            timeSinceLastShot = 0f;
            playerBulletGun.Shoot();
            //Debug.Log("Fire!!!");
        }


        bool changeShotType = CrossPlatformInputManager.GetButtonDown("Tab");

        timeSinceChangeOfShotType += Time.deltaTime;
        if(changeShotType && timeSinceChangeOfShotType >= timeBetweenChangingShotType)
        {
            timeSinceChangeOfShotType = 0f;
            //Change Type of fire shot
            //Debug.Log("Change fire type");
            playerBulletGun.m_FireShotType = ((int)(playerBulletGun.m_FireShotType) == (System.Enum.GetValues(typeof(BulletGun.FireShotType)).Length) - 1) ? (BulletGun.FireShotType) 0 :
                                                (BulletGun.FireShotType)(playerBulletGun.m_FireShotType + 1);
        }
    }
}
