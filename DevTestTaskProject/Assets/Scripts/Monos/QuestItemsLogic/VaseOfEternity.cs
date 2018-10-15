using UnityEngine;

public class VaseOfEternity : QuestItemBaseClass
{   
    private void OnTriggerEnter(Collider col)
    {        
        if (col.gameObject.tag == "Sword" && GameManager.Instance.swordActive)
        {            
            GameManager.Instance.CheckQuestItem(gameObject);            
        }
    }    
}
