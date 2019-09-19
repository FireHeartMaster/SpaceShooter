using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Bullet : MonoBehaviour
{
    [HideInInspector] public Vector3 moveDirection = Vector3.right;

    [SerializeField] public float bulletDamage = 10f;
    protected Vector3 bulletDirection = Vector3.right;
    [SerializeField] protected float bulletSpeed = 30f;

    [HideInInspector] public bool playerBullet = true;

    [HideInInspector] public bool canProvokeDamage = true;

    public virtual void Init(float m_damage, Vector3 m_direction, float m_speed, bool isPlayer)
    {
        bulletDamage = m_damage;
        bulletDirection = m_direction;
        bulletSpeed = m_speed;
        playerBullet = isPlayer;
    }


    public virtual void ShotHit(float timeToDisapear)
    {
        canProvokeDamage = false;
        Explode();

        Destroy(gameObject, timeToDisapear);
    }

    public virtual void Explode()
    {

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((playerBullet && collision.tag == "Enemy") || (!playerBullet && collision.tag == "Player"))
        {
            //provoke damage
            Health health = collision.GetComponent<Health>();

            if (health != null)
            {
                health.TakeDamage(bulletDamage);

                ShotHit(0f);
            }
        }
    }
}
