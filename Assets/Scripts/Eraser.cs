using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eraser : MonoBehaviour
{
    Factory factory;

    private void Start()
    {
        factory = Factory.m_Factory;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag != "Eraser")
        {
            switch (collision.tag)
            {
                case ("Bullet"):
                    SpiralBullet spiralBulletScript = collision.GetComponent<SpiralBullet>();
                    //Debug.Log("(spiralBulletScript != null) -- " + ((spiralBulletScript != null)).ToString());
                    factory.DeactivateAndStoreBullet(collision.gameObject, (spiralBulletScript != null));
                    break;
                default:
                    Destroy(collision.gameObject);
                    break;
            }
        }
    }
}
