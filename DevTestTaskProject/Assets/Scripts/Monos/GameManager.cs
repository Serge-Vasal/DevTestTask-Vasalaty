using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private GameObject[] QuestItemPrefabs;
    [SerializeField] private GameObject PlayerPrefab;
    [SerializeField] private Renderer groundRenderer;
    [SerializeField] private float GroundRandomPointMargin;
    [SerializeField] private float randomPointMinimalDistance;

    private GameObject playerGO;
    private float randomPointMinimalDistanceSqr;
    private List<GameObject> QuestItemsPassivePool;
    private List<GameObject> QuestItemsActivePool;
    private List<Vector3> randomPointsList;
    private Vector3 groundPosition;
    private float groundExtentsX;
    private float groundExtentsZ;
    private int currentQuestItem=0;

    protected override void Awake ()
    {
        base.Awake();
        randomPointMinimalDistanceSqr = randomPointMinimalDistance * randomPointMinimalDistance;
        groundPosition = groundRenderer.gameObject.transform.position;
        groundExtentsX = groundRenderer.bounds.extents.x;
        groundExtentsZ = groundRenderer.bounds.extents.z;
        QuestItemsPassivePool = new List<GameObject>();
        QuestItemsActivePool = new List<GameObject>();
        randomPointsList = new List<Vector3>();

        InstantiatePrefabs();
        SetupScene();     
	}

    void InstantiatePrefabs()
    {
        playerGO = Instantiate(PlayerPrefab);
        playerGO.SetActive(false);

        for (int i = 0; i < QuestItemPrefabs.Length; i++)
        {
            GameObject obj = (GameObject)Instantiate(QuestItemPrefabs[i]);
            obj.SetActive(false);
            QuestItemsPassivePool.Add(obj);
        }
    }

    void SetupScene()
    {
        playerGO.transform.position = FindRandomPoint();
        playerGO.SetActive(true);
        
        for (int i = 0; i <= QuestItemsPassivePool.Count+1; i++)
        {
            GameObject GO = QuestItemsPassivePool[Random.Range(0, QuestItemsPassivePool.Count)];
            QuestItemsActivePool.Add(GO);
            QuestItemsActivePool[i].transform.position = FindRandomPoint();
            QuestItemsActivePool[i].transform.rotation = Quaternion.identity;
            QuestItemsActivePool[i].SetActive(true);
            QuestItemsActivePool[i].gameObject.GetComponent<IQuestable>().OnActivated();            
            
            
            QuestItemsPassivePool.Remove(GO);            
        }       
        
    }

    private Vector3 FindRandomPoint()
    {
        bool isValidRandomPoint=false;
        Vector3 randomPoint=Vector3.zero;

        while (!isValidRandomPoint)
        {
            randomPoint = new Vector3(Random.Range(groundPosition.x - groundExtentsX + GroundRandomPointMargin,
            groundPosition.x + groundExtentsX - GroundRandomPointMargin), 0.0f,
            Random.Range(groundPosition.z - groundExtentsZ + GroundRandomPointMargin,
            groundPosition.z + groundExtentsZ - GroundRandomPointMargin));

            if (randomPointsList.Count > 0)
            {
                for (int i = 0; i < randomPointsList.Count; i++)
                {
                    if ((randomPoint - randomPointsList[i]).sqrMagnitude < randomPointMinimalDistanceSqr)
                    {
                        continue;
                    }
                    else
                    {
                        isValidRandomPoint = true;
                        randomPointsList.Add(randomPoint);
                    }
                }
            }
            else
            {
                isValidRandomPoint = true;
            }
        }
        return randomPoint;
    }

    public void CheckQuestItem(GameObject questItem)
    {
        if (questItem == QuestItemsActivePool[currentQuestItem])
        {
            Debug.Log("True");
            currentQuestItem += 1;
        }
        else
        {
            Debug.Log("False");
        }
    }

    public void UpdateCurrentQuestItem(GameObject questItem)
    {

    }
	
	
}
