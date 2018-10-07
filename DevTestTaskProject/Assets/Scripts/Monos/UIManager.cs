using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private GameObject questItemsHolder;
    [SerializeField] private Image[] questItemSlots= new Image[3];

    private int slotCounter=0;

    public void UpdateQuestItemSlots(Sprite questItemSprite)
    {
        questItemSlots[slotCounter].sprite = questItemSprite;
        slotCounter += 1;
    }

}
