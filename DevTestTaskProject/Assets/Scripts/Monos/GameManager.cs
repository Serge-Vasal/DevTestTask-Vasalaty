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
    [SerializeField] private GameObject[] SystemPrefabs;    

    private GameObject playerGO;
    private float randomPointMinimalDistanceSqr;
    private List<GameObject> QuestItemsPassivePool;
    private List<GameObject> QuestItemsActivePool;
    private List<Vector3> randomPointsList;
    private List<GameObject> instancedSystemPrefabs;
    private Vector3 groundPosition;
    private float groundExtentsX;
    private float groundExtentsZ;
    private int currentQuestItem=0;    

    public bool swordActive=false;

    protected override void Awake ()
    {
        base.Awake();
        instancedSystemPrefabs = new List<GameObject>();
        InstantiateSystemPrefabs();
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

    void InstantiateSystemPrefabs()
    {
        GameObject prefabInstance;
        for (int i = 0; i < SystemPrefabs.Length; ++i)
        {
            prefabInstance = Instantiate(SystemPrefabs[i]);
            instancedSystemPrefabs.Add(prefabInstance);
        }
    }

    void InstantiatePrefabs()
    {
        playerGO = Instantiate(PlayerPrefab);
        playerGO.SetActive(false);        

        for (int i = 0; i < QuestItemPrefabs.Length; i++)
        {
            GameObject obj = (GameObject)Instantiate(QuestItemPrefabs[i]);
            QuestItemBaseClass questScript = obj.GetComponent<QuestItemBaseClass>();
            questScript.InstantiateParticles();
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
            QuestItemsActivePool[i].gameObject.GetComponent<QuestItemBaseClass>().OnActivated();            
            
            QuestItemsPassivePool.Remove(GO);            
        } 
    }

    void UnloadScene()
    {
        UIManager.Instance.ResetUI();
        currentQuestItem = 0;
        randomPointsList.Clear();
        for (int i = 0; i <= QuestItemsActivePool.Count+1; i++)
        {            
            GameObject GO = QuestItemsActivePool[0];
            QuestItemBaseClass questItemBaseScript = GO.GetComponent<QuestItemBaseClass>();
            questItemBaseScript.StopAllParticles();
            questItemBaseScript.DeactivateQuestItem();
            QuestItemsPassivePool.Add(GO);
            QuestItemsActivePool.Remove(GO);                    
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
                        isValidRandomPoint = false;                        
                        break;
                    }
                    isValidRandomPoint = true;                                       
                }

                if (isValidRandomPoint)
                {                    
                    randomPointsList.Add(randomPoint);                    
                    break;
                }  
            }
            else
            {
                isValidRandomPoint = true;                
                randomPointsList.Add(randomPoint);                                       
                break;
            } 
        }        
        return randomPoint;
    }

    public void CheckQuestItem(GameObject questItem)
    {
        QuestItemBaseClass thisQuestBaseScript = questItem.GetComponent<QuestItemBaseClass>();        
        if (questItem == QuestItemsActivePool[currentQuestItem])
        {
            thisQuestBaseScript.isActivatedQuestItem = true;
            thisQuestBaseScript.ActivateParticles(true, questItem.transform);
            currentQuestItem += 1;
            if (currentQuestItem == QuestItemsActivePool.Count)
            {
                StartCoroutine(QuestFinished());
            }
        }
        else
        {
            if (!thisQuestBaseScript.isActivatedQuestItem)
            {
                for (int i = 0; i < QuestItemsActivePool.Count; i++)
                {
                    QuestItemBaseClass questBaseScript = QuestItemsActivePool[i].GetComponent<QuestItemBaseClass>();
                    questBaseScript.StopAllParticles();
                    questBaseScript.DeactivateQuestItem();
                }
                thisQuestBaseScript.ActivateParticles(false, questItem.transform);
                currentQuestItem = 0;
            }            
        }
    }
    
    IEnumerator QuestFinished()
    {        
        yield return new WaitForSeconds(2);
        UnloadScene();
        SetupScene();
    }
	
}
