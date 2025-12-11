using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField] private Transform hookTransform;
    [SerializeField] private float maxHookDistance = 5f;
    [SerializeField] private LineRenderer ropeLine;

    private Letter caughtLetter;
    //public float ropeSpeed;
    public float hookSpeed;
    public float rotationSpeed = 10f;

    private bool isRotating = true;
    private bool isShooting = false;
    private bool isReturning = false;

    private Vector3 hookStartLocalPos;
    private Vector3 shootDirection;


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

        //HOOK EXIT
        if (isShooting == true && hookTransform != null)
        {
            hookTransform.position += shootDirection * hookSpeed * Time.deltaTime;

            float dist = Vector3.Distance(hookTransform.position, transform.position);
            if (dist >= maxHookDistance && caughtLetter == null)
            {
                isShooting = false;
                isReturning = true;
            }
        }


        //RETURN
        if (isReturning == true && hookTransform != null)
        {
            Vector3 targetPos = transform.TransformPoint(hookStartLocalPos);

            hookTransform.position = Vector3.MoveTowards(hookTransform.position, targetPos, hookSpeed * Time.deltaTime);

            if (hookTransform.position == targetPos)
            {
                hookTransform.localPosition = hookStartLocalPos;
                isReturning = false;
                isRotating = true;

                if (caughtLetter != null)
                {
                    Destroy(caughtLetter.gameObject);
                    caughtLetter = null;
                }
            }
        }
        // ROPE BY LINE RENDERER
        if (ropeLine != null)
        {
            ropeLine.SetPosition(0, transform.position);
            ropeLine.SetPosition(1, hookTransform.position);

        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isShooting) return;

        Letter letter = other.GetComponent<Letter>();
        if (letter == null) return;

        if (!letter.isMainLetter) return;

        if (caughtLetter == null)
        {
            caughtLetter = letter;
            caughtLetter.transform.SetParent(hookTransform);

            isShooting = false;
            isReturning = true;
        }
    }


}
