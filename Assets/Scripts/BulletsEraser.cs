using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletsEraser : MonoBehaviour
{
    Factory factory;

    private void Start()
    {
        factory = Factory.m_Factory;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("OnTriggerEnter2D");
        if (collision.tag != "Eraser")
        {
            //Debug.Log("collision.tag != \"Eraser\"");
            //Debug.Log("collision.tag: " + collision.tag.ToString());
            switch (collision.tag)
            {
                case ("Bullet"):
                    Bullet bullet = collision.GetComponent<Bullet>();
                    SpiralBullet spiralBulletScript = collision.GetComponent<SpiralBullet>();
                    if (bullet.playerBullet)
                    {
                        factory.DeactivateAndStoreBullet(collision.gameObject, (spiralBulletScript != null));
                    }
                    //Debug.Log("(spiralBulletScript != null) -- " + ((spiralBulletScript != null)).ToString());
                    break;

                //case ("Enemy"):
                //    factory.DeactivateAndStoreEnemy(collision.gameObject);
                //    break;

                default:
                    //Destroy(collision.gameObject);
                    break;
            }
        }
    }
}
