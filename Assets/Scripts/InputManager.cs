using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class InputManager : MonoBehaviour
{
    [SerializeField] protected MoveShip playerMoveShip;
    [SerializeField] protected BulletGun shipBulletGun;
    //[SerializeField] GameObject simpleBulletPrefab;

    [SerializeField] protected float timeBetweenShots = 0.2f;
    float timeSinceLastShot;

    [SerializeField] protected float timeBetweenChangingShotType = 0.2f;
    float timeSinceChangeOfShotType;

    [SerializeField] protected Energy energy;

    [SerializeField] protected float timeBetweenDodges = 1f;
    float timeSinceLastDodge;

    private void Awake()
    {
        timeSinceLastShot = timeBetweenShots;

        timeSinceChangeOfShotType = timeBetweenChangingShotType;

        timeSinceLastDodge = timeBetweenDodges;
    }

    protected void Start()
    {
        int numberOfBulletTypes = System.Enum.GetValues(typeof(/*BulletGun.*/FireShotType)).Length;

        if (numberOfBulletTypes == shipBulletGun.bulletImageSelection.Length)
        {
            for (int i = 0; i < numberOfBulletTypes; i++)
            {
                if (i == (int)shipBulletGun.m_FireShotType)
                {
                    shipBulletGun.bulletImageSelection[i].color = new Color(shipBulletGun.bulletImageSelection[i].color.r, shipBulletGun.bulletImageSelection[i].color.g, shipBulletGun.bulletImageSelection[i].color.b, 1f);
                }
                else
                {
                    shipBulletGun.bulletImageSelection[i].color = new Color(shipBulletGun.bulletImageSelection[i].color.r, shipBulletGun.bulletImageSelection[i].color.g, shipBulletGun.bulletImageSelection[i].color.b, shipBulletGun.alphaValueWhenNotActive);
                }

            }
        }
    }
    protected void Update()
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
                shipBulletGun.Shoot();
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
            int numberOfBulletTypes = System.Enum.GetValues(typeof(/*BulletGun.*/FireShotType)).Length;
            shipBulletGun.m_FireShotType = ((int)(shipBulletGun.m_FireShotType) == numberOfBulletTypes - 1) ? (/*BulletGun.*/FireShotType) 0 :
                                                (/*BulletGun.*/FireShotType)(shipBulletGun.m_FireShotType + 1);
            if(numberOfBulletTypes == shipBulletGun.bulletImageSelection.Length)
            {
                for(int i=0; i< numberOfBulletTypes; i++)
                {
                    if(i == (int)shipBulletGun.m_FireShotType)
                    {
                        shipBulletGun.bulletImageSelection[i].color = new Color(shipBulletGun.bulletImageSelection[i].color.r, shipBulletGun.bulletImageSelection[i].color.g, shipBulletGun.bulletImageSelection[i].color.b, 1f);
                    }
                    else
                    {
                        shipBulletGun.bulletImageSelection[i].color = new Color(shipBulletGun.bulletImageSelection[i].color.r, shipBulletGun.bulletImageSelection[i].color.g, shipBulletGun.bulletImageSelection[i].color.b, shipBulletGun.alphaValueWhenNotActive);
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
