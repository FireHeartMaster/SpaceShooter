using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public enum MovementType { SimpleLine, Diagonal, Cosine }
public class EnemySimpleInputManager : MonoBehaviour
{
    [SerializeField] protected MoveShipEnemySimple moveShip;
    [SerializeField] protected BulletGun shipBulletGun;
    //[SerializeField] GameObject simpleBulletPrefab;

    [SerializeField] protected float timeBetweenShots = 0.2f;
    float timeSinceLastShot;

    [SerializeField] protected float timeBetweenChangingShotType = 0.2f;
    float timeSinceChangeOfShotType;

    [SerializeField] protected Energy energy;

    [SerializeField] protected float timeBetweenDodges = 1f;
    float timeSinceLastDodge;

    [SerializeField] FireShotType m_FireShotType = FireShotType.Simple;
    [SerializeField] MovementType enemyMovementType = MovementType.SimpleLine;

    [Space]
    [Header("Cosine Angle Movement")]
    float cosineMovementTimeCounter = 0f;
    float cosineMovementPreviousTimeCounter = 0f;
    //[SerializeField] float cosineMovementAngleIncreaseRate = 10f;
    [SerializeField] float cosineMovementAmplitude = 30f;
    [SerializeField] float angularFrequency = 1f;

    [Space]
    [Header("EnemyShooting")]
    [SerializeField] float baseControlledTimeBetweenShots = 3f;
    float randomizedBaseControlledTimeBetweenShots;
    [SerializeField] float maxRandomRangeAroundBaseControlledTimeBetweenShots = 0.5f;


    private void Awake()
    {
        timeSinceLastShot = timeBetweenShots;

        timeSinceChangeOfShotType = timeBetweenChangingShotType;

        timeSinceLastDodge = timeBetweenDodges;

        randomizedBaseControlledTimeBetweenShots = Random.Range(baseControlledTimeBetweenShots - maxRandomRangeAroundBaseControlledTimeBetweenShots, baseControlledTimeBetweenShots + maxRandomRangeAroundBaseControlledTimeBetweenShots);
        //timeSinceChangeOfShotType = randomizedBaseControlledTimeBetweenShots;
    }

    protected void Start()
    {
        shipBulletGun.m_FireShotType = m_FireShotType;
    }
    protected void Update()
    {
        //float w = CrossPlatformInputManager.GetAxis("Vertical");
        //float h = CrossPlatformInputManager.GetAxis("Horizontal");

        //float fire = CrossPlatformInputManager.GetAxis("Jump");       

        switch (enemyMovementType)
        {
            case (MovementType.SimpleLine):
                moveShip.move = Vector3.left;
                break;
            case (MovementType.Diagonal):
                moveShip.move = (Vector3.left + Vector3.up).normalized;
                break;
            case (MovementType.Cosine):
                cosineMovementPreviousTimeCounter = cosineMovementTimeCounter;
                cosineMovementTimeCounter += /*cosineMovementAngleIncreaseRate * */Time.deltaTime;
                moveShip.move = -(new Vector3(angularFrequency * (cosineMovementTimeCounter - cosineMovementPreviousTimeCounter), cosineMovementAmplitude * (Mathf.Sin(angularFrequency * cosineMovementTimeCounter) - Mathf.Sin(angularFrequency * cosineMovementPreviousTimeCounter)), 0f)).normalized;
                break;
            default:
                moveShip.move = Vector3.left;
                
                break;
        }

        //moveShip.move = (new Vector3(h, w, 0f)).normalized;

        float fire = 0f;

        timeSinceLastShot += Time.deltaTime;
        //Debug.Log("timeSinceLastShot: " + timeSinceLastShot);
        //Debug.Log("randomizedBaseControlledTimeBetweenShots: " + randomizedBaseControlledTimeBetweenShots);
        if (timeSinceLastShot >= randomizedBaseControlledTimeBetweenShots)
        {
            fire = 1f;
            //timeSinceLastShot = 0f;
            randomizedBaseControlledTimeBetweenShots = Random.Range(baseControlledTimeBetweenShots - maxRandomRangeAroundBaseControlledTimeBetweenShots, baseControlledTimeBetweenShots + maxRandomRangeAroundBaseControlledTimeBetweenShots);
        }
        //Debug.Log("fire: " + fire);
        if (fire >= 0.5f && timeSinceLastShot >= timeBetweenShots)
        {
            if (energy.canShoot)
            {
                timeSinceLastShot = 0f;
                shipBulletGun.Shoot();
                //Debug.Log("Fire!!!");
                fire = 0f;
            }
        }


        //bool changeShotType = CrossPlatformInputManager.GetButtonDown("Tab");

        bool changeShotType = false;

        timeSinceChangeOfShotType += Time.deltaTime;
        if (changeShotType && timeSinceChangeOfShotType >= timeBetweenChangingShotType)
        {
            timeSinceChangeOfShotType = 0f;
            //Change Type of fire shot
            //Debug.Log("Change fire type");
            int numberOfBulletTypes = System.Enum.GetValues(typeof(/*BulletGun.*/FireShotType)).Length;
            shipBulletGun.m_FireShotType = ((int)(shipBulletGun.m_FireShotType) == numberOfBulletTypes - 1) ? (/*BulletGun.*/FireShotType)0 :
                                                (/*BulletGun.*/FireShotType)(shipBulletGun.m_FireShotType + 1);
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



        //bool dodgeInput = CrossPlatformInputManager.GetButtonDown("Fire1"); //condition to verify input to dodge attacks
        bool dodgeInput = false;

        timeSinceLastDodge += Time.deltaTime;

        if (dodgeInput && timeSinceLastDodge >= timeBetweenDodges && !energy.slowRecover)
        {
            timeSinceLastDodge = 0f;
            //dodge now
            moveShip.Dodge();
        }
    }
}
