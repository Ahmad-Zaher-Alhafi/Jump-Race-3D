using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpObjectBase : MonoBehaviour
{
    [SerializeField] private float distanceToMove;
    [SerializeField] private float movingSpeed;
    [SerializeField] private float rotatingSpeed;
    [SerializeField] private CenterPoint centerPoint;
    [SerializeField] private JumpObject jumpObjectOfThisBase;

    public JumpObject JumpObjectOfThisBase
    {
        get => jumpObjectOfThisBase;
        private set => jumpObjectOfThisBase = value;
    }

    private bool hasToMove;
    private float minDistanceOnX;
    private float maxDistanceOnX;
    private bool hasToRotate;
    private float angleToAdd;
    private bool isItRacerStartObject;
    public bool IsItRacerStartJumpObject//if one of the racers will stand above of this jump object at the begining of the race
    {
        get => isItRacerStartObject;
        set => isItRacerStartObject = value;
    }

    private bool isNotStableObject
    {
        get
        {
            return jumpObjectOfThisBase.IsItPathJumpObject & !isItRacerStartObject & !jumpObjectOfThisBase.IsItLastJumpObject;
        }
    }

    private void Start()
    {
        if (isNotStableObject)
        {
            int rand = Random.Range(0, 5);

            if (rand == 0)
            {
                hasToMove = true;
            }

            minDistanceOnX = transform.position.x - distanceToMove;
            maxDistanceOnX = transform.position.x + distanceToMove;
        }
    }

    private void Update()
    {
        if (hasToMove)
        {
            Move();
        }

        if (hasToRotate)
        {
            Rotate();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == Constances.PlayerLayerNum)
        {
            if (!jumpObjectOfThisBase.IsItLastJumpObject)
            {
                Player.Instance.OnTriggerEnterWithJumpableObject(true, jumpObjectOfThisBase, jumpObjectOfThisBase.IsItPathJumpObject);
            }
        }
        else if (other.gameObject.layer == Constances.RacerLayerNum)
        {
            if (jumpObjectOfThisBase.IsItPathJumpObject)
            {
                other.GetComponent<Racer>().OnTriggerEnterWithJumpableObject(jumpObjectOfThisBase);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == Constances.PlayerLayerNum)
        {
            Player.Instance.OnTriggerExitFromJumpAbleObject();
        }
        else if (other.gameObject.layer == Constances.RacerLayerNum)
        {
            other.GetComponent<Racer>().OnTriggerExitFromJumpAbleObject();
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
        }
    }

    public void Initialize(bool isItPathJumpObject, int jumpObIndex, Color jumpObColor, int pathObjectsCount)
    {
        if (isItPathJumpObject)
        {
            tag = Constances.PathJumpObjectTag;
        }
        else
        {
            tag = "Untagged";
        }

        jumpObjectOfThisBase.Initialize(isItPathJumpObject, jumpObIndex, jumpObColor, pathObjectsCount);
    }

    public JumpObject GetJumpObjectOfThisBase()
    {
        return jumpObjectOfThisBase;
    }

    public void DeactivateCenterPoint()
    {
        centerPoint.DeactivateCenterPoint();
    }
}
