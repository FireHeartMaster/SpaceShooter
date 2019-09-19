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

    [SerializeField] Energy energy;

    [SerializeField] float timeBetweenDodges = 1f;
    float timeSinceLastDodge;

    private void Awake()
    {
        timeSinceLastShot = timeBetweenShots;

        timeSinceChangeOfShotType = timeBetweenChangingShotType;

        timeSinceLastDodge = timeBetweenDodges;
    }

    private void Start()
    {
        int numberOfBulletTypes = System.Enum.GetValues(typeof(BulletGun.FireShotType)).Length;

        if (numberOfBulletTypes == playerBulletGun.bulletImageSelection.Length)
        {
            for (int i = 0; i < numberOfBulletTypes; i++)
            {
                if (i == (int)playerBulletGun.m_FireShotType)
                {
                    playerBulletGun.bulletImageSelection[i].color = new Color(playerBulletGun.bulletImageSelection[i].color.r, playerBulletGun.bulletImageSelection[i].color.g, playerBulletGun.bulletImageSelection[i].color.b, 1f);
                }
                else
                {
                    playerBulletGun.bulletImageSelection[i].color = new Color(playerBulletGun.bulletImageSelection[i].color.r, playerBulletGun.bulletImageSelection[i].color.g, playerBulletGun.bulletImageSelection[i].color.b, playerBulletGun.alphaValueWhenNotActive);
                }

            }
        }
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
            if (energy.canShoot)
            {
                timeSinceLastShot = 0f;
                playerBulletGun.Shoot();
                //Debug.Log("Fire!!!");
            }
        }


        bool changeShotType = CrossPlatformInputManager.GetButtonDown("Tab");

        timeSinceChangeOfShotType += Time.deltaTime;
        if(changeShotType && timeSinceChangeOfShotType >= timeBetweenChangingShotType)
        {
            timeSinceChangeOfShotType = 0f;
            //Change Type of fire shot
            //Debug.Log("Change fire type");
            int numberOfBulletTypes = System.Enum.GetValues(typeof(BulletGun.FireShotType)).Length;
            playerBulletGun.m_FireShotType = ((int)(playerBulletGun.m_FireShotType) == numberOfBulletTypes - 1) ? (BulletGun.FireShotType) 0 :
                                                (BulletGun.FireShotType)(playerBulletGun.m_FireShotType + 1);
            if(numberOfBulletTypes == playerBulletGun.bulletImageSelection.Length)
            {
                for(int i=0; i< numberOfBulletTypes; i++)
                {
                    if(i == (int)playerBulletGun.m_FireShotType)
                    {
                        playerBulletGun.bulletImageSelection[i].color = new Color(playerBulletGun.bulletImageSelection[i].color.r, playerBulletGun.bulletImageSelection[i].color.g, playerBulletGun.bulletImageSelection[i].color.b, 1f);
                    }
                    else
                    {
                        playerBulletGun.bulletImageSelection[i].color = new Color(playerBulletGun.bulletImageSelection[i].color.r, playerBulletGun.bulletImageSelection[i].color.g, playerBulletGun.bulletImageSelection[i].color.b, playerBulletGun.alphaValueWhenNotActive);
                    }

                }
            }
        }



        bool dodgeInput = CrossPlatformInputManager.GetButtonDown("Fire1"); //condition to verify input to dodge attacks

        timeSinceLastDodge += Time.deltaTime;

        if (dodgeInput && timeSinceLastDodge >= timeBetweenDodges && !energy.slowRecover)
        {
            timeSinceLastDodge = 0f;
            //dodge now
            playerMoveShip.Dodge();
        }
    }
}
