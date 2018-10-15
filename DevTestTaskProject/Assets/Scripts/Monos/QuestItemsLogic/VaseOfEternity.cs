using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TPS;

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
