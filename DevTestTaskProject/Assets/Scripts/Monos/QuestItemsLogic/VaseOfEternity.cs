﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VaseOfEternity : MonoBehaviour, IQuestable
{
    [SerializeField] private Sprite itemIcon;
    public void OnActivated()
    {
        UIManager.Instance.UpdateQuestItemSlots(itemIcon);
    }

    void Start () {
		
	}
	
	
	void Update () {
		
	}
}