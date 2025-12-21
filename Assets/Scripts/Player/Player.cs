using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Transform catchAnchor;
    [SerializeField] private Transform hookTransform;
    [SerializeField] private float maxHookDistance = 5f;
    [SerializeField] private LineRenderer ropeLine;

   // private Letter caughtLetter;
    public float hookSpeed;
    public float rotationSpeed = 10f;

    private bool isRotating = true;
    private bool isShooting = false;
    private bool isReturning = false;

    private Vector3 hookStartLocalPos;
    private Vector3 shootDirection;

    public static event Action<string> OnMainLetterDelivered;
    private string carriedMainLetterId = null;

    private Transform carriedLetterTransform = null;



    void Start()
    {
        // RECORD START POSITION
        if (hookTransform != null)
        {
            hookStartLocalPos = hookTransform.localPosition;
        }

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ActivateHook();
        }

        //ROTATION
        if (isRotating == true && hookTransform != null)
        {
            hookTransform.Rotate(0f, 0f, -rotationSpeed * Time.deltaTime); 
        }

        HookExit();
        HookReturn();

        // ROPE BY LINE RENDERER
        if (ropeLine != null)
        {
            ropeLine.SetPosition(0, transform.position);
            ropeLine.SetPosition(1, hookTransform.position);

        }
    }

    private void ActivateHook()
    {
        if (isReturning == false && isRotating == true)
        {
            isRotating = false;
            isShooting = true;
            ShootHook();
        }
    }

    private void ShootHook()
    {
        shootDirection = -hookTransform.up;
    }

    private void ReturnHook()
    {
        isShooting = false;
        isReturning = true;
    }

    private void HookExit()
    {
        if (isShooting == true && hookTransform != null)
        {
            hookTransform.position += shootDirection * hookSpeed * Time.deltaTime;

            //CHANGE BOOLIANS IF DIDNT CATCH A LETTER & CROSSED MaxHookDistance
            float dist = Vector3.Distance(hookTransform.position, transform.position);
            if (dist >= maxHookDistance)
            {
                isShooting = false;
                isReturning = true;
            }
        }
    }

    private void HookReturn()
    {
        if (isReturning == true && hookTransform != null)
        {
            hookTransform.localPosition = Vector3.MoveTowards(hookTransform.localPosition, hookStartLocalPos, hookSpeed * Time.deltaTime);

            if (hookTransform.localPosition == hookStartLocalPos)
            {
                hookTransform.localPosition = hookStartLocalPos;
                isReturning = false;
                isRotating = true;

                if (carriedMainLetterId != null)
                {
                    //TO GAME MANAGER -  *AFTER* RETURNED
                    OnMainLetterDelivered?.Invoke(carriedMainLetterId);
                    carriedMainLetterId = null;

                    if (carriedLetterTransform != null)
                    {
                        Destroy(carriedLetterTransform.gameObject);
                        carriedLetterTransform = null;
                    }

                }
            }
        }
    }

    private void LetterToHookCenter()
    {

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isShooting) 
        {
            return;
        };

        Letter letter = other.GetComponent<Letter>();

        if (letter == null)
        {
            return;
        }
        //CHECK IF MAIN LETTER
        bool shouldCollectAsMain = GameManager.instance.TryGetLetter(letter.letterID);

        if (shouldCollectAsMain == true)
        {
            letter.transform.SetParent(catchAnchor, false);
            letter.transform.localPosition = Vector3.zero;
            letter.transform.localRotation = Quaternion.identity;

            carriedLetterTransform = letter.transform;
            carriedMainLetterId = letter.letterID;
        }
        else
        {
            Debug.Log("Wrong Letter Hit");
        }

        ReturnHook();
    }


}
