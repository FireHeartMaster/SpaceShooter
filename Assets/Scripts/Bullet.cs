using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Bullet : MonoBehaviour
{
    [HideInInspector] public Vector3 moveDirection = Vector3.right;

    [SerializeField] public float bulletDamage = 10f;
    [SerializeField] protected Vector3 bulletDirection = Vector3.right;
    [SerializeField] protected float bulletSpeed = 30f;

    [HideInInspector] public bool playerBullet = true;

    [HideInInspector] public bool canProvokeDamage = true;

    [SerializeField] protected GameObject smallExplosionPrefab;

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
        GameObject smallExplosionGameObject = Instantiate(smallExplosionPrefab, transform.position, Quaternion.identity);
        Destroy(smallExplosionGameObject, 5f);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("playerBullet: " + playerBullet);
        Debug.Log("collision.tag: " + collision.tag);
        Debug.Log("this: " + this);
        if ((playerBullet && collision.tag == "Enemy") || (!playerBullet && collision.tag == "Player"))
        {
            //provoke damage
            Health health = collision.GetComponent<Health>();

            Debug.Log("health == null ??? " + (health == null).ToString());
            if (health != null)
            {
                health.TakeDamage(bulletDamage);

                ShotHit(0f);
            }
        }
    }
}
