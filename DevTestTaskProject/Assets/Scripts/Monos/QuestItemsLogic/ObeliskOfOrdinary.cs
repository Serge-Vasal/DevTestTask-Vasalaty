using System.Collections;
using UnityEngine;

public class ObeliskOfOrdinary : QuestItemBaseClass
{    
    [SerializeField] private Transform playerStartRotationNull;
    [SerializeField] private Transform playerCurrentRotationNull;

    private bool isPlayerInside;
    private Transform playerTransform;
    private Quaternion playerStartRotation;
    private Quaternion playerCurrentRotation;    
    private Vector3 obeliskToPlayerVector;
    private Vector3 obeliskPosition;
    private Vector3 rotationDeltaCurr;
    private Vector3 rotationDeltaPrev;
    private float rotationDeltaFinal;

    private bool ok90=false;
    private bool ok180=false;
    private bool ok270=false;
    
    private bool isInNegativeZone; 

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player" && !isPlayerInside)
        {
            obeliskPosition = transform.position;
            isPlayerInside = true;
            playerTransform = collider.gameObject.transform;
            obeliskToPlayerVector = playerTransform.position - obeliskPosition;  
            playerStartRotation = Quaternion.LookRotation(obeliskToPlayerVector, Vector3.up);
            playerStartRotationNull.rotation = playerStartRotation;
            StartCoroutine(CheckPlayerStatus());
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            isPlayerInside = false;
            isInNegativeZone = false;
            StopAllCoroutines();
            Debug.Log("exited");
                        
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

            if (ok90 && ok90 && ok180 && ok270 && (Mathf.Abs(rotationDeltaFinal) > 355f))
            {
                StopAllCoroutines();
                ok90 = false;
                ok180 = false;
                ok270 = false;                
                GameManager.Instance.CheckQuestItem(gameObject);  
                }
            yield return new WaitForFixedUpdate();
        }
    }
}
