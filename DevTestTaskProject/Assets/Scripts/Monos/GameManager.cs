using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private GameObject[] questItemPrefabs;
    [SerializeField] private GameObject[] systemPrefabs;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Renderer groundRenderer;
    [SerializeField] private float groundRandomPointMargin;
    [SerializeField] private float randomPointMinimalDistance;      

    private GameObject playerGO;
    private float randomPointMinimalDistanceSqr;
    private List<GameObject> questItemsPassivePool;
    private List<GameObject> questItemsActivePool;
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

        questItemsPassivePool = new List<GameObject>();
        questItemsActivePool = new List<GameObject>();
        randomPointsList = new List<Vector3>();

        InstantiatePrefabs();
        SetupScene();     
	}

    void InstantiateSystemPrefabs()
    {
        GameObject prefabInstance;
        for (int i = 0; i < systemPrefabs.Length; ++i)
        {
            prefabInstance = Instantiate(systemPrefabs[i]);
            instancedSystemPrefabs.Add(prefabInstance);
        }
    }

    void InstantiatePrefabs()
    {
        playerGO = Instantiate(playerPrefab);
        playerGO.SetActive(false);        

        for (int i = 0; i < questItemPrefabs.Length; i++)
        {
            GameObject obj = (GameObject)Instantiate(questItemPrefabs[i]);
            QuestItemBaseClass questScript = obj.GetComponent<QuestItemBaseClass>();
            questScript.InstantiateParticles();
            obj.SetActive(false);
            questItemsPassivePool.Add(obj);
        }
    }

    void SetupScene()
    {
        playerGO.transform.position = FindRandomPoint();
        playerGO.SetActive(true);
        
        for (int i = 0; i <= questItemsPassivePool.Count+1; i++)
        {
            GameObject GO = questItemsPassivePool[Random.Range(0, questItemsPassivePool.Count)];
            questItemsActivePool.Add(GO);
            questItemsActivePool[i].transform.position = FindRandomPoint();
            questItemsActivePool[i].transform.rotation = Quaternion.identity;
            questItemsActivePool[i].SetActive(true);
            questItemsActivePool[i].gameObject.GetComponent<QuestItemBaseClass>().OnActivated();            
            
            questItemsPassivePool.Remove(GO);            
        } 
    }

    void UnloadScene()
    {
        UIManager.Instance.ResetUI();
        currentQuestItem = 0;
        randomPointsList.Clear();
        for (int i = 0; i <= questItemsActivePool.Count+1; i++)
        {            
            GameObject GO = questItemsActivePool[0];
            QuestItemBaseClass questItemBaseScript = GO.GetComponent<QuestItemBaseClass>();
            questItemBaseScript.StopAllParticles();
            questItemBaseScript.DeactivateQuestItem();
            questItemsPassivePool.Add(GO);
            questItemsActivePool.Remove(GO);                    
        }
    }

    private Vector3 FindRandomPoint()
    {
        bool isValidRandomPoint=false;
        Vector3 randomPoint=Vector3.zero;

        while (!isValidRandomPoint)
        {
            randomPoint = new Vector3(Random.Range(groundPosition.x - groundExtentsX + groundRandomPointMargin,
            groundPosition.x + groundExtentsX - groundRandomPointMargin), 0.0f,
            Random.Range(groundPosition.z - groundExtentsZ + groundRandomPointMargin,
            groundPosition.z + groundExtentsZ - groundRandomPointMargin));

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
        if (questItem == questItemsActivePool[currentQuestItem])
        {
            thisQuestBaseScript.isActivatedQuestItem = true;
            thisQuestBaseScript.ActivateParticles(true, questItem.transform);
            currentQuestItem += 1;
            if (currentQuestItem == questItemsActivePool.Count)
            {
                StartCoroutine(QuestFinished());
            }
        }
        else
        {
            if (!thisQuestBaseScript.isActivatedQuestItem)
            {
                for (int i = 0; i < questItemsActivePool.Count; i++)
                {
                    QuestItemBaseClass questBaseScript = questItemsActivePool[i].GetComponent<QuestItemBaseClass>();
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
