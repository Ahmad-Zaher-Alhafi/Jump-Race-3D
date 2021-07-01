using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpObjectBase : MonoBehaviour
{
    [SerializeField] private JumpObject jumpObjectOfThisBase;
    [SerializeField] private float distanceToMove;
    [SerializeField] private float movingSpeed;

    private bool isItMovingObject;
    private float minDistanceOnX;
    private float maxDistanceOnX;

    private void Start()
    {
        int rand = Random.Range(0, 5);

        if (rand == 0)
        {
            isItMovingObject = true;
        }

        minDistanceOnX = transform.position.x - distanceToMove;
        maxDistanceOnX = transform.position.x + distanceToMove;
    }

    private void Update()
    {
        if (isItMovingObject)
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
    }

    public JumpObject GetJumpObjectOfThisBase()
    {
        return jumpObjectOfThisBase;
    }
}
