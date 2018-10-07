﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareOfWorship : MonoBehaviour, IQuestable
{
    [SerializeField] private Sprite itemIcon;
    
    private Animator playerAnim;
    private bool isPlayerInside;

    private void Start()
    {
        playerAnim = GameObject.FindWithTag("Player").GetComponent<Animator>();        
    }

    public void OnActivated()
    {
        UIManager.Instance.UpdateQuestItemSlots(itemIcon);   
    }    

    void OnTriggerEnter(Collider collider)
    {        
        if (collider.gameObject.tag == "Player")
        {
            isPlayerInside = true;
            StartCoroutine(CheckPlayerStatus());
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            isPlayerInside = false;            
        }
    }

    IEnumerator CheckPlayerStatus()
    {              
        while (isPlayerInside)
        {  
            if (!playerAnim.GetBool("isMoving"))
            {
                GameManager.Instance.CheckQuestItem(gameObject);
                yield break;
            }            
            yield return new WaitForFixedUpdate();           
        }        
    }
}