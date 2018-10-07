using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObeliskOfOrdinary : MonoBehaviour, IQuestable
{
    [SerializeField] private Sprite itemIcon;

    private bool isPlayerInside;
    private Transform player;
    private Vector3 playerPosition;
    private Vector3 obeliskPosition;
    private Vector3 obeliskToPlayerStartVector;
    private Vector3 obeliskToPlayerCurrVector;
    private float prevAngle;
    private float currAngle;
    

    private void Start()
    {
        obeliskPosition = transform.position;
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
            player = collider.gameObject.transform;
            obeliskToPlayerStartVector = obeliskPosition - player.position;
            StartCoroutine(CheckPlayerStatus());
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            isPlayerInside = false;
            prevAngle = 0f;
            prevAngle = 0f;
        }
    }

    IEnumerator CheckPlayerStatus()
    {
        while (isPlayerInside)
        {
            
            //Debug.Log("prevAngle " + prevAngle);
            //Debug.Log("currAngle " + currAngle);            
            //if()
            //{
                            

            //    GameManager.Instance.CheckQuestItem(gameObject);
            //    yield break;
            //}
            //prevAngle= currAngle;
            

            //if ()
            //{

            //    yield break;
            //}
            yield return new WaitForFixedUpdate();
        }
    }
}
