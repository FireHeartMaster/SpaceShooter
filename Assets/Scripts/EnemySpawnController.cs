using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Xml.Linq;

public class EnemySpawnController : MonoBehaviour
{
    [HideInInspector] public float currentTimeSinceLevelStart = 0f;

    public List<EnemyDataToSpawn> spawnInfo = new List<EnemyDataToSpawn>();

    Factory factory;

    [SerializeField] string pathToXMLFile = "XMLFiles/Level01.xml";
    XDocument xmlDoc;
    //IEnumerable<XElement> items;
    List <XMLData> data = new List <XMLData>(); //Initialize List of XMLData objects.

    [Space]
    [Header("Victory panel")]
    [SerializeField] float timeToWin = 60f;
    [SerializeField] GameObject victoryPanel;
    [SerializeField] InputManager playerInputManager;

    private void Start()
    {
        factory = Factory.m_Factory;
        LoadXML();

        //EnemyDataToSpawn newEnemyDataToSpawn = new EnemyDataToSpawn(0, 1, new Vector3(70.5f, -7f, 34f), 10.5f);
        //Debug.Log("Calling AddEnemySpawnInfoToTheList()");
        //AddEnemySpawnInfoToTheList(newEnemyDataToSpawn);

        //newEnemyDataToSpawn = new EnemyDataToSpawn(2, 0, new Vector3(71f, -5.2f, 34f), 5f);
        //Debug.Log("Calling AddEnemySpawnInfoToTheList()");
        //AddEnemySpawnInfoToTheList(newEnemyDataToSpawn);

        //foreach(EnemyDataToSpawn enemyData in spawnInfo)
        //{
        //    Debug.Log(enemyData.ToString());
        //}
    }
    private void Update()
    {
        currentTimeSinceLevelStart += Time.deltaTime;

        if (spawnInfo.Count > 0)
        {
            //Debug.Log(spawnInfo[0].ToString());
            while (currentTimeSinceLevelStart >= spawnInfo[0].timeOfSpawning)
            {
                factory.CreateEnemy(spawnInfo[0].enemyGroup, spawnInfo[0].enemyIndexInTheGroup, spawnInfo[0].positionToSpawn);
                spawnInfo.RemoveAt(0);
                if(spawnInfo.Count == 0)
                {
                    break;
                }
            }
        }

        if(currentTimeSinceLevelStart >= timeToWin)
        {
            victoryPanel.SetActive(true);
            playerInputManager.enabled = false;
        }
    }

    public void AddEnemySpawnInfoToTheList(EnemyDataToSpawn newEnemySpawnInfo)
    {
        
        if(spawnInfo.Count == 0)
        {
            spawnInfo.Add(newEnemySpawnInfo);
        }
        else
        {
            int currentIndexInTheList = 0;
            while (newEnemySpawnInfo.timeOfSpawning > spawnInfo[currentIndexInTheList].timeOfSpawning)
            {
                currentIndexInTheList++;
                if (currentIndexInTheList >= spawnInfo.Count)
                {
                    break;
                }
            }

            if(currentIndexInTheList >= spawnInfo.Count)
            {
                spawnInfo.Add(newEnemySpawnInfo);
            }
            else
            {
                spawnInfo.Insert(currentIndexInTheList, newEnemySpawnInfo);
            }
        }
    }


    void LoadXML()

    {

        //Assigning Xdocument xmlDoc. Loads the xml file from the file path listed. 
        xmlDoc = XDocument.Load(Application.streamingAssetsPath + "\\" + pathToXMLFile);

        //This basically breaks down the XML Document into XML Elements. Used later. 
        var allData = xmlDoc.Descendants("level").Elements("EnemyDataToSpawn");

        foreach(var data in allData)
        {
            int newEnemyGroup = (int)data.Element("enemyGroup");
            int newEnemyIndexInTheGroup = (int)data.Element("enemyIndexInTheGroup");
            float newEnemyX = (float)data.Element("x");
            float newEnemyY = (float)data.Element("y");
            float newEnemyZ = (float)data.Element("z");
            float newEnemyTimeOfSpawning = (float)data.Element("timeOfSpawning");

            EnemyDataToSpawn newEnemyDataToSpawn = new EnemyDataToSpawn(newEnemyGroup, newEnemyIndexInTheGroup, new Vector3(newEnemyX, newEnemyY, newEnemyZ), newEnemyTimeOfSpawning);
            //Debug.Log("newEnemyDataToSpawn: ");
            //Debug.Log("  group: " + newEnemyDataToSpawn.enemyGroup.ToString());
            //Debug.Log("  index: " + newEnemyDataToSpawn.enemyIndexInTheGroup.ToString());
            //Debug.Log("  position: " + newEnemyDataToSpawn.positionToSpawn.x.ToString() + ", " + newEnemyDataToSpawn.positionToSpawn.y.ToString() + ", " + newEnemyDataToSpawn.positionToSpawn.z.ToString());
            //Debug.Log("  spawning time: " + newEnemyDataToSpawn.timeOfSpawning.ToString());
            //Debug.Log(" ");

            AddEnemySpawnInfoToTheList(newEnemyDataToSpawn);

        }

    }

}

public class EnemyDataToSpawn
{
    public int enemyGroup;
    public int enemyIndexInTheGroup;
    public Vector3 positionToSpawn;
    public float timeOfSpawning;

    public EnemyDataToSpawn(int _enemyGroup, int _enemyIndexInTheGroup, Vector3 _positionToSpawn, float _timeOfSpawning)
    {
        enemyGroup = _enemyGroup;
        enemyIndexInTheGroup = _enemyIndexInTheGroup;
        positionToSpawn = _positionToSpawn;
        timeOfSpawning = _timeOfSpawning;
    }

    public override string ToString()
    {
        string stringToReturn = "EnemyDataToSpawn: \n";

        stringToReturn += "enemyGroup: " + enemyGroup.ToString() + "\n";
        stringToReturn += "enemyIndexInTheGroup: " + enemyIndexInTheGroup.ToString() + "\n";
        stringToReturn += "positionToSpawn: (" + positionToSpawn.x.ToString() + ", " + positionToSpawn.y.ToString() + ", " + positionToSpawn.z.ToString() + ")\n";
        stringToReturn += "timeOfSpawning: " + timeOfSpawning + "\n";
        return stringToReturn;
    }
}


public class XMLData
{

    public int pageNum;

    public string charText, dialogueText;

    public XMLData (int page, string character, string dialogue)
    {

        pageNum = page;

        charText = character;

        dialogueText = dialogue;

    }

}