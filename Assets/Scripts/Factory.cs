using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory : MonoBehaviour
{
    public static Factory m_Factory;

    [Space]
    [Header("Bullets")]

    //Player bullets
    List<GameObject> simpleBullets;
    List<GameObject> spiralBullets;

    [SerializeField] GameObject simpleBulletsParent;
    [SerializeField] GameObject spiralBulletsParent;

    [SerializeField] GameObject simpleBulletPrefab;
    [SerializeField] GameObject spiralBulletPrefab;


    [Space]
    [Header("Explosions")]

    List<GameObject> normalExplosions;
    List<GameObject> smallExplosions;

    [SerializeField] GameObject normalExplosionParent;
    [SerializeField] GameObject smallExplosionParent;

    [SerializeField] GameObject normalExplosionPrefab;
    [SerializeField] GameObject smallExplosionPrefab;


    private void Awake()
    {
        m_Factory = this;

        simpleBullets = new List<GameObject>();
        spiralBullets = new List<GameObject>();

        normalExplosions = new List<GameObject>();
        smallExplosions = new List<GameObject>();
    }

    public GameObject CreateBullet(Vector3 position, Quaternion orientation, bool isSpiralBullet = false)
    {
        List<GameObject> bulletListToUse = !isSpiralBullet ? simpleBullets : spiralBullets;
        GameObject bullet;
        if (bulletListToUse.Count == 0)
        {
            bullet = Instantiate(!isSpiralBullet ? simpleBulletPrefab : spiralBulletPrefab, position, orientation);
            bullet.transform.parent = !isSpiralBullet ? simpleBulletsParent.transform : spiralBulletsParent.transform;
            
        }
        else
        {
            bullet = bulletListToUse[bulletListToUse.Count - 1];
            bulletListToUse.RemoveAt(bulletListToUse.Count - 1);
            bullet.transform.position = position;
            bullet.transform.rotation = orientation;
            //Debug.Log("Spiral Bullet Retaken -- isSpiralBullet: " + isSpiralBullet);
            if (isSpiralBullet)
            {
                //Debug.Log("Spiral Bullet Retaken");
                SpiralBullet spiralBulletScript = bullet.GetComponent<SpiralBullet>();
                spiralBulletScript.currentRadius = 0f;
                spiralBulletScript.currentAngle = 0f;
            }
            bullet.SetActive(true);

        }

        return bullet;
    } 

    public void DeactivateAndStoreBullet(GameObject bullet, bool isSpiralBullet = false)
    {
        List<GameObject> bulletListToUse = !isSpiralBullet ? simpleBullets : spiralBullets;
        bulletListToUse.Add(bullet);
        bullet.SetActive(false);

    }



    public GameObject CreateExplosion(Vector3 position, Quaternion orientation, bool isSmallExplosion = false)
    {
        List<GameObject> explosionListToUse = !isSmallExplosion ? normalExplosions : smallExplosions;
        GameObject explosion;
        if (explosionListToUse.Count == 0)
        {
            explosion = Instantiate(!isSmallExplosion ? normalExplosionPrefab : smallExplosionPrefab, position, orientation);
            explosion.transform.parent = !isSmallExplosion ? normalExplosionParent.transform : smallExplosionParent.transform;

        }
        else
        {
            explosion = explosionListToUse[explosionListToUse.Count - 1];
            explosionListToUse.RemoveAt(explosionListToUse.Count - 1);
            explosion.transform.position = position;
            explosion.transform.rotation = orientation;

            explosion.SetActive(true);
            Animator explosionAnimator = explosion.GetComponent<Animator>();
            explosionAnimator.Play("ExplosionAnimation");

        }

        return explosion;
    }

    public void DeactivateAndStoreExplosion(GameObject explosion, bool isSmallExplosion = false)
    {
        Debug.Log("DeactivateAndStoreExplosion");
        List<GameObject> explosionListToUse = !isSmallExplosion ? normalExplosions : smallExplosions;
        explosionListToUse.Add(explosion);
        explosion.SetActive(false);

    }

    public void CallIntentToDeactivateExplosion(GameObject explosion, bool isSmallExplosion, float timeToDeactivate = 5f)
    {
        IEnumerator coroutine = IntentToDeactivateExplosion(explosion, isSmallExplosion);
        StartCoroutine(coroutine);
    }
    IEnumerator IntentToDeactivateExplosion(GameObject explosion, bool isSmallExplosion, float timeToDeactivate = 5f)
    {
        Debug.Log("DeactivateExplosion");
        Debug.Log("timeToDeactivate: " + timeToDeactivate.ToString());
        yield return new WaitForSeconds(timeToDeactivate);
        Debug.Log("Calling factory.DeactivateAndStoreExplosion(explosion, isSmallExplosion);");
        DeactivateAndStoreExplosion(explosion, isSmallExplosion);

    }

}
