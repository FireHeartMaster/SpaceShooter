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

    protected bool isSpiralBullet = false;

    [Space]
    [Header("Pool Factory")]
    Factory factory;

    private void Start()
    {
        factory = Factory.m_Factory;
    }

    public virtual void Init(float m_damage, Vector3 m_direction, float m_speed, bool isPlayer, bool _isSpiralBullet = false)
    {
        bulletDamage = m_damage;
        bulletDirection = m_direction;
        bulletSpeed = m_speed;
        playerBullet = isPlayer;
        isSpiralBullet = _isSpiralBullet;
    }


    public virtual void ShotHit(float timeToDisapear)
    {
        canProvokeDamage = false;
        Explode();

        //Destroy(gameObject, timeToDisapear);
        factory.DeactivateAndStoreBullet(gameObject, isSpiralBullet);

    }

    public virtual void Explode()
    {
        Debug.Log("Explode");
        //GameObject smallExplosionGameObject = Instantiate(smallExplosionPrefab, transform.position, Quaternion.identity);
        GameObject smallExplosionGameObject = factory.CreateExplosion(transform.position, Quaternion.identity, true);
        //Destroy(smallExplosionGameObject, 5f);

        //Invoke("factory.DeactivateAndStoreExplosion(smallExplosionGameObject, true)", 5f);
        //IEnumerator coroutineToDeactivateExplosion = DeactivateExplosion(smallExplosionGameObject, true);
        //StartCoroutine(coroutineToDeactivateExplosion);

        factory.CallIntentToDeactivateExplosion(smallExplosionGameObject, true);

    }

    //IEnumerator DeactivateExplosion(GameObject explosion, bool isSmallExplosion, float timeToDeactivate = 5f)
    //{
    //    Debug.Log("DeactivateExplosion");
    //    Debug.Log("timeToDeactivate: " + timeToDeactivate.ToString());
    //    yield return new WaitForSeconds(1f);
    //    Debug.Log("Calling factory.DeactivateAndStoreExplosion(explosion, isSmallExplosion);");
    //    factory.DeactivateAndStoreExplosion(explosion, isSmallExplosion);

    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("playerBullet: " + playerBullet);
        //Debug.Log("collision.tag: " + collision.tag);
        //Debug.Log("this: " + this);
        if ((playerBullet && collision.tag == "Enemy") || (!playerBullet && collision.tag == "Player"))
        {
            //provoke damage
            Health health = collision.GetComponent<Health>();

            //Debug.Log("health == null ??? " + (health == null).ToString());
            if (health != null)
            {
                health.TakeDamage(bulletDamage);

                ShotHit(0f);
            }
        }
    }
}
