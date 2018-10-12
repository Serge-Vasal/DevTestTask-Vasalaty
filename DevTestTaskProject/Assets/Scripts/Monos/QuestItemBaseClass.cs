using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestItemBaseClass : MonoBehaviour
{
    [SerializeField] private GameObject fireworksOfHappinessPrefab;
    [SerializeField] private GameObject cloudOfDispairPrefab;
    [SerializeField] private Sprite itemIcon;

    private GameObject fireworksOfHappinessInst;
    private GameObject cloudOfDispairInst;
    private ParticleSystem fireworksOfHappinessParticles;
    private ParticleSystem cloudOfDispairParticles;

    public bool isActivatedQuestItem=false;

    public void OnActivated()
    {
        UIManager.Instance.UpdateQuestItemSlots(itemIcon);
    }   

    public void InstantiateParticles()
    {
        fireworksOfHappinessInst = (GameObject)Instantiate(fireworksOfHappinessPrefab);
        cloudOfDispairInst = (GameObject)Instantiate(cloudOfDispairPrefab);        
        fireworksOfHappinessParticles = fireworksOfHappinessInst.GetComponent<ParticleSystem>();
        fireworksOfHappinessParticles.Stop();
        cloudOfDispairParticles = cloudOfDispairInst.GetComponent<ParticleSystem>();
        cloudOfDispairParticles.Stop();
    }  

    public void ActivateParticles(bool isSuccess,Transform questItemposition)
    {
        if (isSuccess)
        {
            StopAllParticles();
            fireworksOfHappinessInst.transform.position = new Vector3(questItemposition.position.x, 0.25f,
                questItemposition.position.z) ;            
            fireworksOfHappinessInst.SetActive(true);
            fireworksOfHappinessParticles.Play();
        }
        else
        {
            StopAllParticles();
            cloudOfDispairInst.transform.position = new Vector3(questItemposition.position.x, 0.35f,
                questItemposition.position.z);
            cloudOfDispairInst.SetActive(true);
            cloudOfDispairParticles.Play();
        }
    }

    public void StopAllParticles()
    {        
        cloudOfDispairParticles.Stop();
        fireworksOfHappinessParticles.Stop();
        cloudOfDispairInst.SetActive(false);
        fireworksOfHappinessInst.SetActive(false);
    }

    public void DeactivateQuestItem()
    {
        isActivatedQuestItem = false;
    }


}
