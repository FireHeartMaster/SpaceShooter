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

    [Space]
    [Header("Enemy Spawner")]

    //List<List<GameObject>> enemies;

    //[SerializeField] GameObject[] enemyPrefabs;

    //GameObject[] enemyParents;

    [SerializeField] GameObject groupParent;
    [SerializeField] GameObject enemyParent;

    [SerializeField] GroupOfEnemies[] groupsOfEnemies;

    [Space]
    [SerializeField] bool spawnEnemy = false;
    [SerializeField] int enemyGroup = 0;
    [SerializeField] int enemyIndexInTheGroup = 0;

    private void Awake()
    {
        m_Factory = this;

        simpleBullets = new List<GameObject>();
        spiralBullets = new List<GameObject>();

        normalExplosions = new List<GameObject>();
        smallExplosions = new List<GameObject>();


        ////////////////////////////////
        //enemyParents = new GameObject[enemyPrefabs.Length];
        //for(int i=0; i < enemyPrefabs.Length; i++)
        //{
        //    enemyParents[i] = Instantiate(emptyGameObjectPrefab, transform.position, Quaternion.identity);
        //}

        //enemies = new List<List<GameObject>>(enemyPrefabs.Length);
        //for(int i=0; i< enemyPrefabs.Length; i++)
        //{
        //    enemies[i] = new List<GameObject>();
        //}

        /////////////////////////////
        for (int i = 0; i < groupsOfEnemies.Length; i++)
        {
            groupsOfEnemies[i].groupParent = Instantiate(groupParent, transform.position, Quaternion.identity);
            groupsOfEnemies[i].groupParent.transform.parent = transform;
            groupsOfEnemies[i].enemyParents = new GameObject[groupsOfEnemies[i].enemyPrefabs.Length];

            groupsOfEnemies[i].enemies = new List<List<GameObject>>(/*groupsOfEnemies[i].enemyPrefabs.Length*/);
            groupsOfEnemies[i].enemyParents = new GameObject[groupsOfEnemies[i].enemyPrefabs.Length];

            for (int j = 0; j < groupsOfEnemies[i].enemyPrefabs.Length; j++)
            {
                groupsOfEnemies[i].enemies.Add(new List<GameObject>());
                //groupsOfEnemies[i].enemies[j] = new List<GameObject>();
                groupsOfEnemies[i].enemyParents[j] = Instantiate(enemyParent, transform.position, Quaternion.identity);
                groupsOfEnemies[i].enemyParents[j].transform.parent = groupsOfEnemies[i].groupParent.transform;
            }
        }

    }

    private void Update()
    {
        if (spawnEnemy)
        {
            spawnEnemy = false;
            if (enemyGroup >= 0 && enemyGroup < groupsOfEnemies.Length){
                if(enemyIndexInTheGroup >= 0 && enemyIndexInTheGroup < groupsOfEnemies[enemyGroup].enemyPrefabs.Length)
                {
                    CreateEnemy(enemyGroup, enemyIndexInTheGroup, transform.position);
                }
                else
                {
                    Debug.Log("Enemy index out of bounds");
                }
            }
        }
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
        //Debug.Log("DeactivateAndStoreExplosion");
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
        //Debug.Log("DeactivateExplosion");
        //Debug.Log("timeToDeactivate: " + timeToDeactivate.ToString());
        yield return new WaitForSeconds(timeToDeactivate);
        //Debug.Log("Calling factory.DeactivateAndStoreExplosion(explosion, isSmallExplosion);");
        DeactivateAndStoreExplosion(explosion, isSmallExplosion);

    }

    public void CreateEnemy(int groupIndex, int indexInTheGroup, Vector3 position)
    {
        GameObject enemy;
        if (groupsOfEnemies[groupIndex].enemies[indexInTheGroup].Count == 0)
        {
            enemy = Instantiate(groupsOfEnemies[groupIndex].enemyPrefabs[indexInTheGroup], position, groupsOfEnemies[groupIndex].enemyPrefabs[indexInTheGroup].transform.rotation);
            //groupsOfEnemies[groupIndex].enemies[indexInTheGroup].Add(enemy);
            enemy.transform.parent = groupsOfEnemies[groupIndex].enemyParents[indexInTheGroup].transform;
        }
        else
        {
            enemy = groupsOfEnemies[groupIndex].enemies[indexInTheGroup][groupsOfEnemies[groupIndex].enemies[indexInTheGroup].Count - 1];
            groupsOfEnemies[groupIndex].enemies[indexInTheGroup].RemoveAt(groupsOfEnemies[groupIndex].enemies[indexInTheGroup].Count - 1);

            Health enemyHealth = enemy.GetComponent<Health>();
            enemyHealth.ReinitializeHealth();
            Energy enemyEnergy = enemy.GetComponent<Energy>();
            enemyEnergy.ReinitializeEnergy();

            enemy.transform.position = position;
            enemy.SetActive(true);
        }

        EnemySpawnInfo enemySpawnInfo = enemy.GetComponent<EnemySpawnInfo>();
        //Debug.Log("enemySpawnInfo == null: " + (enemySpawnInfo == null).ToString());
        enemySpawnInfo.enemyGroupIndex = groupIndex;
        enemySpawnInfo.enemyIndexInTheGroup = indexInTheGroup;
    }

    public void DeactivateAndStoreEnemy(GameObject enemy/*, int groupIndex, int indexInTheGroup*/)
    {
        enemy.SetActive(false);
        EnemySpawnInfo enemySpawnInfo = enemy.GetComponent<EnemySpawnInfo>();
        groupsOfEnemies[enemySpawnInfo.enemyGroupIndex].enemies[enemySpawnInfo.enemyIndexInTheGroup].Add(enemy);
    }

}


[System.Serializable]
public class GroupOfEnemies
{
    public GameObject groupParent;

    public GameObject[] enemyPrefabs;

    public List<List<GameObject>> enemies;

    [HideInInspector] public GameObject[] enemyParents;


}