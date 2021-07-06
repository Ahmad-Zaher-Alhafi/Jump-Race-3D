using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpObjectBase : MonoBehaviour
{
    [SerializeField] private JumpObject jumpObjectOfThisBase;
    [SerializeField] private float distanceToMove;
    [SerializeField] private float movingSpeed;
    [SerializeField] private float rotatingSpeed;
    [SerializeField] private CenterPoint centerPoint;

    private bool isItMovingObject;
    private float minDistanceOnX;
    private float maxDistanceOnX;
    private bool hasToRotate;
    private float angleToAdd;
    private bool isItRacerStartObject;
    public bool IsItRacerStartObject//if one of the racers will stand above of this jump object at the begining of the race
    {
        get => isItRacerStartObject;
        set => isItRacerStartObject = value;
    }

    private void Start()
    {
        if (jumpObjectOfThisBase.IsItPathJumpObject && !isItRacerStartObject)
        {
            int rand = Random.Range(0, 5);

            if (rand == 0)
            {
                isItMovingObject = true;
            }

            minDistanceOnX = transform.position.x - distanceToMove;
            maxDistanceOnX = transform.position.x + distanceToMove;
        }
    }

    private void Update()
    {
        if (isItMovingObject)
        {
            Move();
        }

        if (hasToRotate)
        {
            Rotate();
        }
    }

    private void Move()
    {
        if (transform.position.x < minDistanceOnX)
        {
            transform.position = new Vector3(minDistanceOnX, transform.position.y, transform.position.z);
            movingSpeed *= -1;
        }
        else if (transform.position.x > maxDistanceOnX)
        {
            transform.position = new Vector3(maxDistanceOnX, transform.position.y, transform.position.z);
            movingSpeed *= -1;
        }

        transform.Translate(transform.right * movingSpeed * Time.deltaTime);
    }

    public void OrderToRotate()
    {
        hasToRotate = true;
        centerPoint.gameObject.SetActive(false);
    }

    private void Rotate()
    {
        angleToAdd += rotatingSpeed * Time.deltaTime;

        if (angleToAdd < 360)
        {
            transform.eulerAngles = Vector3.Slerp(transform.eulerAngles, Vector3.forward * angleToAdd, rotatingSpeed * Time.deltaTime);
        }
        else
        {
            hasToRotate = false;
            angleToAdd = 0;
            transform.eulerAngles = Vector3.forward * 360;

            if (tag == Constances.PathJumpObjectTag && jumpObjectOfThisBase.JumpObjectIndex != 0)
            {
                centerPoint.gameObject.SetActive(true);
            }
            
        }
    }

    public JumpObject GetJumpObjectOfThisBase()
    {
        return jumpObjectOfThisBase;
    }
}
