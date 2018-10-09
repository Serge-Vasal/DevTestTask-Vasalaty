using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObeliskOfOrdinary : MonoBehaviour, IQuestable
{
    [SerializeField] private Sprite itemIcon;
    [SerializeField] private Transform playerLookRotationNull;
    [SerializeField] private Transform playerCurrentRotationNull;

    private bool isPlayerInside;
    private Transform playerTransform;
    private Quaternion playerStartRotation;
    private Vector3 playerStartRotationEuler;
    private Quaternion playerCurrentRotation;
    private float currentAngle;
    private Vector3 obeliskToPlayerVector;
    private Vector3 obeliskPosition;
    private Vector3 rotationDeltaCurr;
    private Vector3 rotationDeltaPrev;
    private float rotationDeltaFinal;

    private bool ok0 = false;
    private bool ok90=false;
    private bool ok180=false;
    private bool ok270=false;
    private bool ok360=false;
    
    private bool isInNegativeZone;

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
            playerTransform = collider.gameObject.transform;
            obeliskToPlayerVector = playerTransform.position - obeliskPosition;  
            playerStartRotation = Quaternion.LookRotation(obeliskToPlayerVector, Vector3.up);
            playerStartRotationEuler = playerStartRotation.eulerAngles;
            playerLookRotationNull.rotation = playerStartRotation;
            Debug.Log("Start  "+playerStartRotationEuler);
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
            obeliskToPlayerVector= playerTransform.position - obeliskPosition;
            playerCurrentRotation = Quaternion.LookRotation(obeliskToPlayerVector, Vector3.up);
            playerCurrentRotationNull.rotation = playerCurrentRotation;
            rotationDeltaCurr = playerCurrentRotationNull.localEulerAngles;

            if(rotationDeltaCurr.y>350 && !ok270)
            {
                isInNegativeZone = true;
            }
            else if (rotationDeltaCurr.y < 90 && !ok90)
            {
                isInNegativeZone = false;
            }



            if (isInNegativeZone)
            {
                rotationDeltaFinal = rotationDeltaCurr.y - 360f;
            }
            else
            {
                rotationDeltaFinal = rotationDeltaCurr.y;

            }


            if (Mathf.Abs(rotationDeltaFinal) > 90f)
            {
                ok90 = true;
            }
            if(ok90 && (Mathf.Abs(rotationDeltaFinal) < 90f))
            {
                ok90 = false;
            }

            if (Mathf.Abs(rotationDeltaFinal) > 180f)
            {
                ok180 = true;
            }
            if (ok180 && (Mathf.Abs(rotationDeltaFinal) < 180f))
            {
                ok180 = false;
            }

            if (Mathf.Abs(rotationDeltaFinal) > 270f)
            {
                ok270 = true;
            }
            if (ok270 && (Mathf.Abs(rotationDeltaFinal) < 270f))
            {
                ok270 = false;
            }

            if(ok90 && ok90 && ok180 && ok270 && (Mathf.Abs(rotationDeltaFinal) > 355f))
            {
                GameManager.Instance.CheckQuestItem(gameObject);
                yield break;
            }


            Debug.Log(Mathf.Abs(rotationDeltaFinal));
            Debug.Log("ok90  " + ok90);
            Debug.Log("ok180  "+ok180);
            Debug.Log("ok270  "+ok270);            
            





            //if (rotationDeltaCurr.y > 0f )
            //{
            //    ok0 = true;
            //}


            //if (rotationDeltaCurr.y > 0f && !ok0)
            //{
            //    rotationDeltaFinal = rotationDeltaCurr.y - 360f;
            //}
            //if (rotationDeltaCurr.y > 190f && !ok180)
            //{
            //    rotationDeltaFinal=rotationDeltaCurr.y - 360f;
            //}
            //if (rotationDeltaCurr.y > 100f && !ok90)
            //else
            //{
            //    rotationDeltaFinal = rotationDeltaCurr.y;
            //}

            //if (Mathf.Abs(rotationDeltaFinal) > 180f)
            //{
            //    ok180 = true;               
            //}
            //if(ok180 && Mathf.Abs(rotationDeltaFinal) < 180)
            //{
            //    ok180 = false;
            //}
            //if (Mathf.Abs(rotationDeltaFinal) > 270f)
            //{
            //    ok270 = true;               
            //}
            //if (ok270 && Mathf.Abs(rotationDeltaFinal) < 270)
            //{
            //    ok270 = false;
            //}
            //if(ok180 && ok270 && Mathf.Abs(rotationDeltaFinal) > 350f)
            //{
            //    GameManager.Instance.CheckQuestItem(gameObject);
            //    yield break;
            //}


            //if(rotationDelta.y+10f>= 350f)
            //{

            //}
            //Debug.Log("Delta  " + rotationDelta);

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
